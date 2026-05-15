namespace FsFlow.Tests

open System
open System.Collections.Generic
open FsFlow
open FsFlow.Capabilities.Core
open Swensen.Unquote
open Xunit

module RuntimeFoundationTests =
    type private Service =
        { Name: string }

    type private AppDependencies =
        { Value: int
          DeviceClient: TestSupport.IDeviceClient }

    type private RuntimeContract =
        { Clock: IClock
          Log: ILog }

    [<Fact>]
    let ``runtime registry stores tagged services independently`` () =
        use scope = new Scope()

        let registry =
            Registry.empty scope
            |> Registry.add<Service> None { Name = "default" }
            |> Registry.add<Service> (Some "Main") { Name = "main" }
            |> Registry.add<Service> (Some "Audit") { Name = "audit" }

        test <@ Registry.get<Service> None registry = { Name = "default" } @>
        test <@ Registry.get<Service> (Some "Main") registry = { Name = "main" } @>
        test <@ Registry.get<Service> (Some "Audit") registry = { Name = "audit" } @>
        test <@ Registry.tryGet<Service> (Some "Missing") registry = None @>

    [<Fact>]
    let ``runtime registry replace only changes the addressed slot`` () =
        use scope = new Scope()

        let registry =
            Registry.empty scope
            |> Registry.add<Service> None { Name = "default" }
            |> Registry.add<Service> (Some "Main") { Name = "main" }
            |> Registry.replace<Service> (Some "Main") { Name = "main-2" }

        test <@ Registry.get<Service> None registry = { Name = "default" } @>
        test <@ Registry.get<Service> (Some "Main") registry = { Name = "main-2" } @>

    [<Fact>]
    let ``runtime registry missing lookup fails intentionally`` () =
        use scope = new Scope()

        let registry = Registry.empty scope

        let failed =
            try
                Registry.get<Service> (Some "Missing") registry |> ignore
                false
            with :? KeyNotFoundException ->
                true

        test <@ failed @>

    [<Fact>]
    let ``scope finalizers run in reverse order and only once`` () =
        let calls = ResizeArray<string>()

        let scope = new Scope()
        scope.AddFinalizer(fun () -> calls.Add "first")
        scope.AddFinalizer(fun () -> calls.Add "second")
        scope.AddFinalizer(fun () -> calls.Add "third")

        (scope :> IDisposable).Dispose()
        (scope :> IDisposable).Dispose()

        test <@ List.ofSeq calls = [ "third"; "second"; "first" ] @>

    [<Fact>]
    let ``runtime adapter projects tagged registry services into a nominal contract`` () =
        let scope = new Scope()
        let clock = Clock.fromValue (DateTimeOffset(2026, 5, 15, 9, 30, 0, TimeSpan.Zero))
        let logMessages = ResizeArray<string>()
        let envVars =
            { new IEnvironmentVariables with
                member _.TryGet name = if name = "FSFLOW_APP_TEST" then Some "1" else None }

        let logger =
            { new ILog with
                member _.Info message = logMessages.Add message }

        let registry =
            Registry.empty scope
            |> Registry.add<IClock> None clock
            |> Registry.add<ILog> (Some "Main") logger
            |> Registry.add<IEnvironmentVariables> None envVars

        let adapter (reg: Registry) =
            {
                Clock = Registry.get<IClock> None reg
                Log = Registry.get<ILog> (Some "Main") reg
            }

        let env = RuntimeAdapter.provide adapter registry

        test <@ env.Clock.UtcNow() = DateTimeOffset(2026, 5, 15, 9, 30, 0, TimeSpan.Zero) @>
        env.Log.Info "hello"
        test <@ List.ofSeq logMessages = [ "hello" ] @>

    [<Fact>]
    let ``runtime helpers use ambient overrides`` () =
        let clock = Clock.fromValue (DateTimeOffset(2026, 5, 15, 10, 0, 0, TimeSpan.Zero))
        let logMessages = ResizeArray<string>()
        let logger =
            { new ILog with
                member _.Info message = logMessages.Add message }
        let random = Random.fromValue 42
        let guid = Guid.fromValue (System.Guid.Parse "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
        let envVars =
            EnvironmentVariables.fromPairs [ "FSFLOW_TEST", "value" ]

        let workflow : Flow<AppDependencies, EnvironmentVariableError, string> =
            flow {
                let! now = Clock.now
                let formattedNow = now.ToString("HH:mm")
                do! Log.info $"now={formattedNow}"
                let! next = Random.nextInt 1 10
                let! id = Guid.newGuid
                let! value = EnvironmentVariable.get "FSFLOW_TEST"
                let! app = Flow.env
                return $"{app.Value}:{next}:{id}:{value}"
            }

        let result =
            workflow
            |> Flow.withClock clock
            |> Flow.withLog logger
            |> Flow.withRandom random
            |> Flow.withGuid guid
            |> Flow.withEnvironmentVariables envVars
            |> Flow.runSync { Value = 7; DeviceClient = { new TestSupport.IDeviceClient with member _.Name = "client" } }

        test <@ result = Exit.Success "7:42:aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa:value" @>
        test <@ List.ofSeq logMessages = [ "now=10:00" ] @>

    [<Fact>]
    let ``environment variable helpers validate and parse values`` () =
        let workflow : Flow<unit, EnvironmentVariableError, int> =
            EnvironmentVariable.getInt "FSFLOW_INT_TEST"

        let result =
            workflow
            |> Flow.withEnvironmentVariables (EnvironmentVariables.fromPairs [ "FSFLOW_INT_TEST", "123" ])
            |> Flow.runSync ()

        test <@ result = Exit.Success 123 @>

    [<Fact>]
    let ``runtime contract can be used as a plain record`` () =
        let contract =
            {
                Clock = Clock.fromValue (DateTimeOffset(2026, 5, 15, 11, 0, 0, TimeSpan.Zero))
                Log = Log.live
            }

        test <@ contract.Clock.UtcNow() = DateTimeOffset(2026, 5, 15, 11, 0, 0, TimeSpan.Zero) @>
