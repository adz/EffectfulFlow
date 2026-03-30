open System
open System.Threading
open System.Threading.Tasks
open EffectFs

type TestFailure(message: string) =
    inherit Exception(message)

module Assert =
    let equal<'value when 'value: equality> (expected: 'value) (actual: 'value) : unit =
        if actual <> expected then
            raise (TestFailure(sprintf "Expected %A but got %A." expected actual))

module Tests =
    let run (name: string) (test: unit -> unit) : bool =
        try
            test ()
            printfn "[pass] %s" name
            true
        with error ->
            eprintfn "[fail] %s: %s" name error.Message
            false

    let effectExpressionBindsValues () : unit =
        let workflow : Effect<unit, string, int> =
            effect {
                let! value = Effect.succeed 40
                let! other = Effect.succeed 2
                return value + other
            }

        let result =
            workflow
            |> Effect.execute ()
            |> Async.RunSynchronously

        Assert.equal (Ok 42) result

    let askReturnsTheEnvironment () : unit =
        let workflow : Effect<int, string, int> =
            effect {
                let! value = Effect.ask<int, string>
                return value * 2
            }

        let result =
            workflow
            |> Effect.execute 21
            |> Async.RunSynchronously

        Assert.equal (Ok 42) result

    let readProjectsFromTheEnvironment () : unit =
        let workflow : Effect<string, string, int> =
            Effect.read String.length

        let result =
            workflow
            |> Effect.execute "effect"
            |> Async.RunSynchronously

        Assert.equal (Ok 6) result

    let ofResultLiftsValidationFailures () : unit =
        let validatePort (value: int) : Result<int, string> =
            if value > 0 then Ok value else Error "port must be positive"

        let result =
            validatePort 0
            |> Effect.ofResult
            |> Effect.execute ()
            |> Async.RunSynchronously

        Assert.equal (Error "port must be positive") result

    let taskInteropRemainsColdUntilExecution () : unit =
        let started = ref false

        let workflow : Effect<unit, string, int> =
            Effect.ofTask(fun (_: CancellationToken) ->
                started.Value <- true
                Task.FromResult 42)

        Assert.equal false started.Value

        let result =
            workflow
            |> Effect.execute ()
            |> Async.RunSynchronously

        Assert.equal true started.Value
        Assert.equal (Ok 42) result

[<EntryPoint>]
let main _ =
    let results =
        [ Tests.run "effect expression binds values" Tests.effectExpressionBindsValues
          Tests.run "ask returns the environment" Tests.askReturnsTheEnvironment
          Tests.run "read projects from the environment" Tests.readProjectsFromTheEnvironment
          Tests.run "ofResult lifts validation failures" Tests.ofResultLiftsValidationFailures
          Tests.run "task interop remains cold until execution" Tests.taskInteropRemainsColdUntilExecution ]

    if List.forall id results then 0 else 1
