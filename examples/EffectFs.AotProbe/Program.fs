open System
open System.Threading
open System.Threading.Tasks
open EffectFs

type ProbeFailure(message: string) =
    inherit Exception(message)

type AsyncFlag() =
    let disposed = ref false

    member _.Disposed = disposed

    interface IAsyncDisposable with
        member _.DisposeAsync() =
            disposed.Value <- true
            ValueTask()

module Assert =
    let equal<'value when 'value: equality> (expected: 'value) (actual: 'value) =
        if actual <> expected then
            raise (ProbeFailure(sprintf "Expected %A but got %A." expected actual))

    let true' (value: bool) =
        equal true value

let run (name: string) (probe: unit -> unit) =
    try
        probe ()
        printfn "[pass] %s" name
        true
    with error ->
        eprintfn "[fail] %s: %s" name error.Message
        false

let execute<'env, 'error, 'value> (environment: 'env) (workflow: Effect<'env, 'error, 'value>) =
    workflow
    |> Effect.execute environment
    |> Async.RunSynchronously

let plainEffectWorks () =
    let result =
        effect {
            let! name = Ok "Ada"
            return $"Hello {name}"
        }
        |> execute ()

    Assert.equal (Ok "Hello Ada") result

let taskInteropWorks () =
    let seen = ref CancellationToken.None

    let result =
        Effect.fromTask(fun cancellationToken ->
            seen.Value <- cancellationToken
            Task.FromResult 42)
        |> execute ()

    Assert.equal (Ok 42) result
    Assert.equal CancellationToken.None seen.Value

let timeoutReturnsTypedError () =
    let result =
        Effect.fromTask(fun cancellationToken ->
            task {
                do! Task.Delay(50, cancellationToken)
                return 42
            })
        |> Effect.timeout (TimeSpan.FromMilliseconds 10.0) "timed out"
        |> execute ()

    Assert.equal (Error "timed out") result

let retryRepeatsUntilSuccess () =
    let attempts = ref 0

    let result =
        Effect.delay(fun () ->
            attempts.Value <- attempts.Value + 1

            if attempts.Value < 3 then
                Effect.fail "retry"
            else
                Effect.succeed 42)
        |> Effect.retry
            { MaxAttempts = 3
              Delay = fun _ -> TimeSpan.Zero
              ShouldRetry = ((=) "retry") }
        |> execute ()

    Assert.equal (Ok 42) result
    Assert.equal 3 attempts.Value

let asyncCleanupRuns () =
    let resource = AsyncFlag()

    let result =
        Effect.usingAsync resource (fun _ -> Effect.succeed 42)
        |> execute ()

    Assert.equal (Ok 42) result
    Assert.true' resource.Disposed.Value

let loggingWorks () =
    let sink = ResizeArray<string>()

    let result =
        Effect.log (fun (messages: ResizeArray<string>) entry -> messages.Add(entry.Message)) LogLevel.Information "hello"
        |> execute sink

    Assert.equal (Ok ()) result
    Assert.equal [ "hello" ] (List.ofSeq sink)

[<EntryPoint>]
let main _ =
    let results =
        [ run "plain effect works" plainEffectWorks
          run "task interop works" taskInteropWorks
          run "timeout returns typed error" timeoutReturnsTypedError
          run "retry repeats until success" retryRepeatsUntilSuccess
          run "async cleanup runs" asyncCleanupRuns
          run "logging works" loggingWorks ]

    if List.forall id results then 0 else 1
