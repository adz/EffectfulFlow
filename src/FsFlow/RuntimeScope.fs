namespace FsFlow

open System
open System.Collections.Generic

/// <summary>
/// Owns finalizers for resources acquired inside a layer or runtime boundary.
/// </summary>
/// <remarks>
/// The scope is internal runtime machinery. It guarantees deterministic teardown and prevents
/// double-disposal by tracking whether cleanup has already happened.
/// </remarks>
type internal Scope() =
    let gate = obj()
    let finalizers = ResizeArray<unit -> unit>()
    let mutable disposed = false

    /// <summary>Registers a finalizer to run when the scope is disposed.</summary>
    member _.AddFinalizer(finalizer: unit -> unit) =
        lock gate (fun () ->
            if disposed then
                raise (ObjectDisposedException(nameof Scope))
            else
                finalizers.Add finalizer)

    interface IDisposable with
        member _.Dispose() =
            let snapshot =
                lock gate (fun () ->
                    if disposed then
                        [||]
                    else
                        disposed <- true
                        finalizers.ToArray())

            let errors = ResizeArray<exn>()

            for index = snapshot.Length - 1 downto 0 do
                try
                    snapshot[index]()
                with ex ->
                    errors.Add ex

            match errors.Count with
            | 0 -> ()
            | 1 -> raise errors[0]
            | _ -> raise (AggregateException("One or more scope finalizers failed.", errors))
