namespace FsFlow

#if !FABLE_COMPILER
open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks

/// <summary>
/// Internal interface for transactional references used by the STM engine.
/// </summary>
type ITRef =
    abstract Id: int64
    abstract Commit: obj -> unit
    abstract CurrentValue: obj

/// <summary>
/// Represents a transactional reference that can be updated atomically within an <see cref="T:FsFlow.STM`1" /> transaction.
/// </summary>
/// <typeparam name="T">The type of the value stored in the reference.</typeparam>
type TRef<'T>(initialValue: 'T) =
    static let mutable idCounter = 0L
    let id = Interlocked.Increment(&idCounter)
    let mutable value = initialValue
    
    interface ITRef with
        member _.Id = id
        member _.Commit(v) = value <- unbox<'T> v
        member _.CurrentValue = box value
        
    member internal _.Id = id
    member internal _.Value = value

/// <summary>
/// Internal journal used to track transactional changes.
/// </summary>
type TJournal = Dictionary<int64, obj * ITRef>

type TransactionResult<'T> =
    | Done of 'T
    | Retry

type TContext =
    {
        Journal: TJournal
        Reads: HashSet<int64>
    }

/// <summary>
/// Represents a transactional operation that can be composed, retried, and executed atomically.
/// </summary>
/// <typeparam name="T">The type of the value produced by the operation.</typeparam>
type STM<'T> = STM of (TContext -> TransactionResult<'T>)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module TRef =
    /// <summary>Creates a new <see cref="T:FsFlow.TRef`1" /> with the initial value.</summary>
    let make (value: 'T) : STM<TRef<'T>> =
        STM(fun _ -> Done(TRef(value)))

    /// <summary>Reads the current value of the transactional reference within a transaction.</summary>
    let get (tref: TRef<'T>) : STM<'T> =
        STM(fun context ->
            context.Reads.Add tref.Id |> ignore

            match context.Journal.TryGetValue tref.Id with
            | true, (v, _) -> Done(unbox<'T> v)
            | false, _ -> Done tref.Value)

    /// <summary>Sets the value of the transactional reference within a transaction.</summary>
    let set (value: 'T) (tref: TRef<'T>) : STM<unit> =
        STM(fun context ->
            context.Journal[tref.Id] <- (box value, tref :> ITRef)
            Done ())

    /// <summary>Updates the value of the transactional reference within a transaction using the supplied function.</summary>
    let update (f: 'T -> 'T) (tref: TRef<'T>) : STM<unit> =
        STM(fun context ->
            context.Reads.Add tref.Id |> ignore

            let current =
                match context.Journal.TryGetValue tref.Id with
                | true, (v, _) -> unbox<'T> v
                | false, _ -> tref.Value

            context.Journal[tref.Id] <- (box (f current), tref :> ITRef)
            Done ())

/// <summary>
/// Computation expression builder for STM transactions.
/// </summary>
type StmBuilder() =
    member _.Return(value: 'T) : STM<'T> = STM(fun _ -> Done value)
    member _.ReturnFrom(stm: STM<'T>) : STM<'T> = stm
    member _.Bind(stm: STM<'T>, f: 'T -> STM<'U>) : STM<'U> =
        STM(fun context ->
            let (STM op) = stm
            match op context with
            | Retry -> Retry
            | Done value ->
                let (STM nextOp) = f value
                nextOp context)
    member _.Zero() : STM<unit> = STM(fun _ -> Done ())
    member _.Delay(f: unit -> STM<'T>) : STM<'T> = STM(fun context -> let (STM op) = f () in op context)
    member _.Combine(stm1: STM<unit>, stm2: STM<'T>) : STM<'T> =
        STM(fun context ->
            let (STM op1) = stm1
            let (STM op2) = stm2
            match op1 context with
            | Retry -> Retry
            | Done () -> op2 context)

[<AutoOpen>]
module StmBuilders =
    /// <summary>
    /// The <c>stm { }</c> computation expression for building atomic transactions.
    /// </summary>
    let stm = StmBuilder()

[<RequireQualifiedAccess>]
module STM =
    let private stmLock = obj()
    let mutable private version = 0L

    let private snapshot (context: TContext) =
        {
            Journal = TJournal(context.Journal)
            Reads = HashSet<int64>(context.Reads)
        }

    let private freshContext () =
        {
            Journal = TJournal()
            Reads = HashSet<int64>()
        }

    /// <summary>Signals that the current branch should retry once observed state changes.</summary>
    let retry<'T> : STM<'T> =
        STM(fun _ -> Retry)

    /// <summary>Tries the left branch and falls back to the right branch when the left branch retries.</summary>
    let orElse (left: STM<'T>) (right: STM<'T>) : STM<'T> =
        STM(fun context ->
            let (STM leftOp) = left
            match leftOp(snapshot context) with
            | Done value -> Done value
            | Retry ->
                let (STM rightOp) = right
                rightOp(snapshot context))

    /// <summary>
    /// Executes an STM transaction atomically within a flow while preserving retry/orElse coordination.
    /// </summary>
    /// <param name="transaction">The STM transaction to execute.</param>
    /// <returns>A flow that performs the transaction and returns its result.</returns>
    let atomically (transaction: STM<'T>) : Flow<'env, 'none, 'T> =
        let rec run () =
            let outcome, _ =
                lock stmLock (fun () ->
                    let (STM op) = transaction
                    let context = freshContext ()

                    match op context with
                    | Done result ->
                        for KeyValue(_, (v, tref)) in context.Journal do
                            tref.Commit(v)

                        version <- version + 1L
                        Monitor.PulseAll stmLock
                        Choice1Of2 result, 0L
                    | Retry ->
                        Choice2Of2 version, version)

            match outcome with
            | Choice1Of2 result ->
                EffectFlow.ofValue result
            | Choice2Of2 versionToWaitFor ->
                lock stmLock (fun () ->
                    while version = versionToWaitFor do
                        Monitor.Wait stmLock |> ignore)

                run ()

        Flow(fun _ _ -> run ())
#endif
