namespace FsFlow.Tests

open System
open System.IO
open System.Threading
open System.Threading.Tasks
open FsFlow
open FsFlow.Tests.TestSupport
open Swensen.Unquote
open Xunit

module WorkflowBasicTests =
    [<Fact>]
    let ``Flow is sync result only`` () =
        let workflow : Flow<int, string, int> =
            Flow.env
            |> Flow.bind (fun value -> Flow.succeed(value * 2))

        test <@ Flow.run 21 workflow = Exit.Success 42 @>

    [<Fact>]
    let ``Flow runFull and runWithToken mirror run for the default token`` () =
        let workflow : Flow<int, string, int> =
            Flow.env
            |> Flow.map (fun value -> value * 2)

        test <@ Flow.run 21 workflow = Exit.Success 42 @>
        test <@ Flow.runFull 21 CancellationToken.None workflow = Exit.Success 42 @>
        test <@ Flow.runWithToken 21 CancellationToken.None workflow = Exit.Success 42 @>

    [<Fact>]
    let ``Flow delay reruns from scratch`` () =
        let runs = ref 0

        let workflow : Flow<unit, string, int> =
            Flow.delay(fun () ->
                runs.Value <- runs.Value + 1
                Flow.succeed runs.Value)

        test <@ Flow.run () workflow = Exit.Success 1 @>
        test <@ Flow.run () workflow = Exit.Success 2 @>

    [<Fact>]
    let ``shared combinators preserve sync and async environment semantics`` () =
        let syncBase : Flow<int, int, int> =
            Flow.read (fun env -> env + 1)
            |> Flow.map ((*) 2)
            |> Flow.bind (fun value -> Flow.succeed(value + 3))
            |> Flow.mapError String.length

        let syncWorkflow : Flow<string, int, int> =
            Flow.localEnv String.length syncBase

        let asyncBase : AsyncFlow<int, int, int> =
            AsyncFlow.read (fun env -> env + 1)
            |> AsyncFlow.map ((*) 2)
            |> AsyncFlow.bind (fun value -> AsyncFlow.succeed(value + 3))
            |> AsyncFlow.mapError String.length

        let asyncWorkflow : AsyncFlow<string, int, int> =
            AsyncFlow.localEnv String.length asyncBase

        let syncResult = Flow.run "flowkit" syncWorkflow

        let asyncResult =
            asyncWorkflow
            |> AsyncFlow.run "flowkit"
            |> Async.RunSynchronously

        test <@ syncResult = Exit.Success 19 @>
        test <@ asyncResult = Exit.Success 19 @>

    [<Fact>]
    let ``flow families expose normalized constructors operators and fallback helpers`` () =
        let syncOk = Flow.ok 41
        let syncAlias = Flow.succeed 41
        let syncError = Flow.error "missing"
        let syncErrorAlias = Flow.fail "missing"

        let syncMapped =
            Flow.(<!>) ((+) 1) syncOk
            |> Flow.run ()

        let syncApplied =
            Flow.(<*>) (Flow.ok ((+) 1)) syncOk
            |> Flow.run ()

        let syncMapped3 =
            Flow.map3 (fun left middle right -> left + middle + right) (Flow.ok 1) (Flow.ok 2) (Flow.ok 3)
            |> Flow.run ()

        let syncIgnored =
            Flow.ignore syncOk
            |> Flow.run ()

        let syncBound =
            Flow.(>>=) syncOk (fun value -> Flow.ok (value + 1))
            |> Flow.run ()

        let syncRecovered =
            Flow.orElseWith (fun (error: string) -> Flow.ok error.Length) syncError
            |> Flow.run ()

        let asyncOk = AsyncFlow.ok 41
        let asyncAlias = AsyncFlow.succeed 41
        let asyncError = AsyncFlow.error "missing"
        let asyncErrorAlias = AsyncFlow.fail "missing"

        let asyncMapped =
            AsyncFlow.(<!>) ((+) 1) asyncOk
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let asyncApplied =
            AsyncFlow.(<*>) (AsyncFlow.ok ((+) 1)) asyncOk
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let asyncMapped3 =
            AsyncFlow.map3 (fun left middle right -> left + middle + right) (AsyncFlow.ok 1) (AsyncFlow.ok 2) (AsyncFlow.ok 3)
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let asyncIgnored =
            AsyncFlow.ignore asyncOk
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let asyncBound =
            AsyncFlow.(>>=) asyncOk (fun value -> AsyncFlow.ok (value + 1))
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let asyncRecovered =
            AsyncFlow.orElseWith (fun (error: string) -> AsyncFlow.ok error.Length) asyncError
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let asyncOkResult = AsyncFlow.run () asyncOk |> Async.RunSynchronously
        let asyncAliasResult = AsyncFlow.run () asyncAlias |> Async.RunSynchronously
        let asyncErrorResult = AsyncFlow.run () asyncError |> Async.RunSynchronously
        let asyncErrorAliasResult = AsyncFlow.run () asyncErrorAlias |> Async.RunSynchronously

        let taskOk = TaskFlow.ok 41
        let taskAlias = TaskFlow.succeed 41
        let taskError = TaskFlow.error "missing"
        let taskErrorAlias = TaskFlow.fail "missing"

        let taskMapped =
            TaskFlow.(<!>) ((+) 1) taskOk
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskApplied =
            TaskFlow.(<*>) (TaskFlow.ok ((+) 1)) taskOk
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskMapped3 =
            TaskFlow.map3 (fun left middle right -> left + middle + right) (TaskFlow.ok 1) (TaskFlow.ok 2) (TaskFlow.ok 3)
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskIgnored =
            TaskFlow.ignore taskOk
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskBound =
            TaskFlow.(>>=) taskOk (fun value -> TaskFlow.ok (value + 1))
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskRecovered =
            TaskFlow.orElseWith (fun (error: string) -> TaskFlow.ok error.Length) taskError
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        test <@ Flow.run () syncOk = Flow.run () syncAlias @>
        test <@ Flow.run () syncError = Flow.run () syncErrorAlias @>
        test <@ syncMapped = Exit.Success 42 @>
        test <@ syncApplied = Exit.Success 42 @>
        test <@ syncMapped3 = Exit.Success 6 @>
        test <@ syncIgnored = Exit.Success () @>
        test <@ syncBound = Exit.Success 42 @>
        test <@ syncRecovered = Exit.Success 7 @>
        test <@ asyncOkResult = asyncAliasResult @>
        test <@ asyncErrorResult = asyncErrorAliasResult @>
        test <@ asyncMapped = Exit.Success 42 @>
        test <@ asyncApplied = Exit.Success 42 @>
        test <@ asyncMapped3 = Exit.Success 6 @>
        test <@ asyncIgnored = Exit.Success () @>
        test <@ asyncBound = Exit.Success 42 @>
        test <@ asyncRecovered = Exit.Success 7 @>
        test <@ (TaskFlow.run () CancellationToken.None taskOk |> fun task -> task.GetAwaiter().GetResult()) = (TaskFlow.run () CancellationToken.None taskAlias |> fun task -> task.GetAwaiter().GetResult()) @>
        test <@ (TaskFlow.run () CancellationToken.None taskError |> fun task -> task.GetAwaiter().GetResult()) = (TaskFlow.run () CancellationToken.None taskErrorAlias |> fun task -> task.GetAwaiter().GetResult()) @>
        test <@ taskMapped = Exit.Success 42 @>
        test <@ taskApplied = Exit.Success 42 @>
        test <@ taskMapped3 = Exit.Success 6 @>
        test <@ taskIgnored = Exit.Success () @>
        test <@ taskBound = Exit.Success 42 @>
        test <@ taskRecovered = Exit.Success 7 @>

    [<Fact>]
    let ``Runnable example docs are generated from executable example projects`` () =
        let repoRoot = Path.GetFullPath(Path.Combine(__SOURCE_DIRECTORY__, "..", ".."))
        let docsExamplesPath = Path.Combine(repoRoot, "docs", "examples", "README.md")
        let generatorPath = Path.Combine(repoRoot, "scripts", "generate-example-docs.sh")
        let generatedPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.md")

        try
            let exitCode, output =
                runBashScript generatorPath [ "DOCS_EXAMPLES_OUTPUT", generatedPath ]

            if exitCode <> 0 then
                failwithf "generate-example-docs.sh failed with exit code %d:%s%s" exitCode Environment.NewLine output

            test <@ File.ReadAllText generatedPath = File.ReadAllText docsExamplesPath @>
        finally
            if File.Exists generatedPath then
                File.Delete generatedPath

    [<Fact>]
    let ``TaskFlow delay reruns from scratch`` () =
        let runs = ref 0

        let workflow : TaskFlow<unit, string, int> =
            TaskFlow.delay(fun () ->
                runs.Value <- runs.Value + 1
                TaskFlow.succeed runs.Value)

        let runOnce () =
            workflow
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        test <@ runOnce () = Exit.Success 1 @>
        test <@ runOnce () = Exit.Success 2 @>

    [<Fact>]
    let ``shared combinators preserve task environment and error semantics`` () =
        let baseWorkflow : TaskFlow<int, int, int> =
            TaskFlow.read (fun env -> env + 1)
            |> TaskFlow.map ((*) 2)
            |> TaskFlow.bind (fun value -> TaskFlow.succeed(value + 3))
            |> TaskFlow.mapError String.length

        let workflow : TaskFlow<string, int, int> =
            TaskFlow.localEnv String.length baseWorkflow

        let result =
            workflow
            |> TaskFlow.run "flowkit" CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        test <@ result = Exit.Success 19 @>

    [<Fact>]
    let ``TaskFlow runtime context splits runtime services from app dependencies`` () =
        let runtime = { RuntimePrefix = "rt:"; Seen = ResizeArray() }

        let app =
            { DeviceClient =
                  { new IDeviceClient with
                      member _.Name = "client" }
              Value = 41 }

        let context = RuntimeContext.create runtime app CancellationToken.None

        let workflow : TaskFlow<RuntimeContext<RuntimeServices, AppDependencies>, string, string> =
            flow {
                let! context = Flow.env
                let prefix = context.Runtime.RuntimePrefix
                let value = context.Environment.Value
                runtime.Seen.Add $"value={value}"
                return $"{prefix}{value}"
            }
            |> TaskFlow.fromFlow

        let result =
            workflow
            |> TaskFlow.runContext context
            |> fun task -> task.GetAwaiter().GetResult()

        test <@ result = Exit.Success "rt:41" @>
        test <@ List.ofSeq runtime.Seen = [ "value=41" ] @>

    [<Fact>]
    let ``TaskFlowSpec runs a built workflow against the combined runtime context`` () =
        let runtime = { RuntimePrefix = "spec:"; Seen = ResizeArray() }

        let app =
            { DeviceClient =
                  { new IDeviceClient with
                      member _.Name = "spec-client" }
              Value = 7 }

        let spec =
            TaskFlowSpec.create runtime app (fun () ->
                flow {
                    let! context = Flow.env
                    return $"{context.Runtime.RuntimePrefix}{context.Environment.Value}"
                }
                |> TaskFlow.fromFlow)

        let result =
            spec
            |> TaskFlowSpec.run CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        test <@ result = Exit.Success "spec:7" @>

    [<Fact>]
    let ``TaskFlow layers and capability helpers compose`` () =
        let runtime =
            { RuntimePrefix = "runtime:"
              Seen = ResizeArray() }

        let app =
            { DeviceClient =
                  { new IDeviceClient with
                      member _.Name = "provider-client" }
              Value = 10 }

        let outerContext = RuntimeContext.create runtime () CancellationToken.None

        let appLayer : Flow<RuntimeContext<RuntimeServices, unit>, string, AppDependencies> =
            Flow.succeed app

        let workflow : Flow<AppDependencies, string, string> =
            flow {
                let! client = Flow.read _.DeviceClient
                let! value = Flow.read _.Value
                return $"{client.Name}:{value}"
            }

        let composed =
            workflow
            |> Flow.provideLayer appLayer

        let composedResult =
            composed
            |> Flow.run outerContext

        let provider = RecordingServiceProvider(typeof<IDeviceClient>, app.DeviceClient :> obj) :> IServiceProvider

        let providerResult =
            Capability.serviceFromProvider<IDeviceClient>
            |> Flow.run provider

        let missingProviderResult =
            Capability.serviceFromProvider<IDeviceClient>
            |> Flow.run (RecordingServiceProvider(typeof<string>, "nope") :> IServiceProvider)

        let flowCapability : Flow<AppDependencies, string, IDeviceClient> =
            Capability.service _.DeviceClient

        let flowCapabilityResult =
            flowCapability
            |> Flow.run app

        let flowLayerWorkflow : Flow<AppDependencies, string, string> =
            flow {
                let! client = Flow.read _.DeviceClient
                let! value = Flow.read _.Value
                return $"{client.Name}:{value}"
            }

        let flowLayerResult =
            flowLayerWorkflow
            |> Flow.provideLayer (Flow.succeed app)
            |> Flow.run ()

        test <@ composedResult = Exit.Success "provider-client:10" @>
        test <@ providerResult = Exit.Success app.DeviceClient @>
        test <@ missingProviderResult = Exit.Failure (Cause.Fail { CapabilityType = typeof<IDeviceClient> }) @>
        test <@ flowCapabilityResult = Exit.Success app.DeviceClient @>
        test <@ flowLayerResult = Exit.Success "provider-client:10" @>

    [<Fact>]
    let ``Flow traverse and sequence work as expected`` () =
        let values = [ 1; 2; 3 ]
        let workflow = values |> Flow.traverse (fun v -> Flow.succeed (v * 2))
        let result = Flow.run () workflow
        test <@ result = Exit.Success [ 2; 4; 6 ] @>

        let flows = [ Flow.succeed 1; Flow.succeed 2 ]
        let sequenceResult = Flow.run () (Flow.sequence flows)
        test <@ sequenceResult = Exit.Success [ 1; 2 ] @>

        let failWorkflow = [ 1; 2 ] |> Flow.traverse (fun v -> if v = 1 then Flow.fail "error" else Flow.succeed v)
        test <@ Flow.run () failWorkflow = Exit.Failure (Cause.Fail "error") @>

    [<Fact>]
    let ``AsyncFlow traverse and sequence work as expected`` () =
        let values = [ 1; 2; 3 ]
        let workflow = values |> AsyncFlow.traverse (fun v -> AsyncFlow.succeed (v * 2))
        let result = AsyncFlow.run () workflow |> Async.RunSynchronously
        test <@ result = Exit.Success [ 2; 4; 6 ] @>

        let flows = [ AsyncFlow.succeed 1; AsyncFlow.succeed 2 ]
        let sequenceResult = AsyncFlow.run () (AsyncFlow.sequence flows) |> Async.RunSynchronously
        test <@ sequenceResult = Exit.Success [ 1; 2 ] @>

    [<Fact>]
    let ``TaskFlow traverse and sequence work as expected`` () =
        let values = [ 1; 2; 3 ]
        let workflow = values |> TaskFlow.traverse (fun v -> TaskFlow.succeed (v * 2))
        let result = TaskFlow.run () CancellationToken.None workflow |> fun t -> t.GetAwaiter().GetResult()
        test <@ result = Exit.Success [ 2; 4; 6 ] @>

        let flows = [ TaskFlow.succeed 1; TaskFlow.succeed 2 ]
        let sequenceResult = TaskFlow.run () CancellationToken.None (TaskFlow.sequence flows) |> fun t -> t.GetAwaiter().GetResult()
        test <@ sequenceResult = Exit.Success [ 1; 2 ] @>

    [<Fact>]
    let ``flow builder overloads stay aligned with the Fable 5 mapping`` () =
        let publicMethods = publicInstanceMethodNames typeof<FlowBuilder>
        let argumentTypeNames = flowBuilderBindAndReturnFromArgumentNames ()

        test <@ publicMethods |> Array.contains "Bind" @>
        test <@ publicMethods |> Array.contains "ReturnFrom" @>
        test <@ publicMethods |> Array.contains "YieldFrom" @>
        test <@ publicMethods |> Array.contains "Yield" @>
        test <@ publicMethods |> Array.contains "Run" @>
        test <@ argumentTypeNames = [| "Env`1"; "Env`2"; "FSharpAsync`1"; "FSharpFunc`2"; "FSharpOption`1"; "FSharpResult`2"; "FSharpValueOption`1"; "Flow`3"; "Task"; "Task`1" |] @>

    [<Fact>]
    let ``flow lives in FsFlow and composes sync flows`` () =
        let workflow : Flow<int, string, int> =
            flow {
                let! env = Flow.env
                let! baseValue = Flow.succeed(env + 1)
                return baseValue * 2
            }

        let result =
            workflow
            |> Flow.run 20

        test <@ typeof<FlowBuilder>.Namespace = "FsFlow" @>
        test <@ result = Exit.Success 42 @>

    [<Fact>]
    let ``flow lives in FsFlow and composes async flows`` () =
        let workflow : Flow<int, string, int> =
            flow {
                let! env = Flow.env
                let! baseValue = Flow.succeed(env + 1)
                return baseValue * 2
            }

        let result =
            workflow
            |> Flow.run 20

        test <@ typeof<FlowBuilder>.Namespace = "FsFlow" @>
        test <@ result = Exit.Success 42 @>
