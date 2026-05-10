namespace FsFlow.Tests

open System
open System.IO
open System.Threading
open System.Threading.Tasks
open FsFlow
open FsFlow.Tests.TestSupport
open Swensen.Unquote
open Xunit

module WorkflowErrorTests =
    [<Fact>]
    let ``Flow composition helpers cover error tapping fallback and pairing`` () =
        let tappedErrors = ResizeArray<string>()

        let tapPreservesOriginalError =
            Flow.fail "primary"
            |> Flow.tapError (fun error ->
                tappedErrors.Add error
                Flow.succeed ())
            |> Flow.run ()

        let tapSkipsSuccess =
            Flow.succeed 42
            |> Flow.tapError (fun error ->
                tappedErrors.Add $"unexpected:{error}"
                Flow.succeed ())
            |> Flow.run ()

        let recovered =
            Flow.fail "missing"
            |> Flow.orElse (Flow.read (fun env -> env + 1))
            |> Flow.run 41

        let bypassesFallback =
            Flow.succeed 10
            |> Flow.orElse (Flow.succeed 99)
            |> Flow.run ()

        let zipped =
            Flow.zip (Flow.read (fun env -> env + 1)) (Flow.read (fun env -> env * 2))
            |> Flow.run 5

        let mapped =
            Flow.map2 (+) (Flow.read (fun env -> env + 1)) (Flow.read (fun env -> env * 2))
            |> Flow.run 5

        test <@ tapPreservesOriginalError = Exit.Failure (Cause.Fail "primary") @>
        test <@ tapSkipsSuccess = Exit.Success 42 @>
        test <@ List.ofSeq tappedErrors = [ "primary" ] @>
        test <@ recovered = Exit.Success 42 @>
        test <@ bypassesFallback = Exit.Success 10 @>
        test <@ zipped = Exit.Success(6, 10) @>
        test <@ mapped = Exit.Success 16 @>

    [<Fact>]
    let ``AsyncFlow composition helpers cover error tapping fallback and pairing`` () =
        let tappedErrors = ResizeArray<string>()

        let tapPreservesOriginalError =
            AsyncFlow.fail "primary"
            |> AsyncFlow.tapError (fun error ->
                tappedErrors.Add error
                AsyncFlow.succeed ())
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let tapSkipsSuccess =
            AsyncFlow.succeed 42
            |> AsyncFlow.tapError (fun error ->
                tappedErrors.Add $"unexpected:{error}"
                AsyncFlow.succeed ())
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let recovered =
            AsyncFlow.fail "missing"
            |> AsyncFlow.orElse (AsyncFlow.read (fun env -> env + 1))
            |> AsyncFlow.run 41
            |> Async.RunSynchronously

        let bypassesFallback =
            AsyncFlow.succeed 10
            |> AsyncFlow.orElse (AsyncFlow.succeed 99)
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let zipped =
            AsyncFlow.zip (AsyncFlow.read (fun env -> env + 1)) (AsyncFlow.read (fun env -> env * 2))
            |> AsyncFlow.run 5
            |> Async.RunSynchronously

        let mapped =
            AsyncFlow.map2 (+) (AsyncFlow.read (fun env -> env + 1)) (AsyncFlow.read (fun env -> env * 2))
            |> AsyncFlow.run 5
            |> Async.RunSynchronously

        test <@ tapPreservesOriginalError = Exit.Failure (Cause.Fail "primary") @>
        test <@ tapSkipsSuccess = Exit.Success 42 @>
        test <@ List.ofSeq tappedErrors = [ "primary" ] @>
        test <@ recovered = Exit.Success 42 @>
        test <@ bypassesFallback = Exit.Success 10 @>
        test <@ zipped = Exit.Success(6, 10) @>
        test <@ mapped = Exit.Success 16 @>

    [<Fact>]
    let ``TaskFlow composition helpers cover error tapping fallback and pairing`` () =
        let tappedErrors = ResizeArray<string>()

        let tapPreservesOriginalError =
            TaskFlow.fail "primary"
            |> TaskFlow.tapError (fun error ->
                tappedErrors.Add error
                TaskFlow.succeed ())
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let tapSkipsSuccess =
            TaskFlow.succeed 42
            |> TaskFlow.tapError (fun error ->
                tappedErrors.Add $"unexpected:{error}"
                TaskFlow.succeed ())
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let recovered =
            TaskFlow.fail "missing"
            |> TaskFlow.orElse (TaskFlow.read (fun env -> env + 1))
            |> TaskFlow.run 41 CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let bypassesFallback =
            TaskFlow.succeed 10
            |> TaskFlow.orElse (TaskFlow.succeed 99)
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let zipped =
            TaskFlow.zip (TaskFlow.read (fun env -> env + 1)) (TaskFlow.read (fun env -> env * 2))
            |> TaskFlow.run 5 CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let mapped =
            TaskFlow.map2 (+) (TaskFlow.read (fun env -> env + 1)) (TaskFlow.read (fun env -> env * 2))
            |> TaskFlow.run 5 CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        test <@ tapPreservesOriginalError = Exit.Failure (Cause.Fail "primary") @>
        test <@ tapSkipsSuccess = Exit.Success 42 @>
        test <@ List.ofSeq tappedErrors = [ "primary" ] @>
        test <@ recovered = Exit.Success 42 @>
        test <@ bypassesFallback = Exit.Success 10 @>
        test <@ zipped = Exit.Success(6, 10) @>
        test <@ mapped = Exit.Success 16 @>

    [<Fact>]
    let ``Check bridges into flow, async, and task shapes`` () =
        let flowBridge =
            Check.okIf false
            |> Flow.orElseFlow (Flow.read (fun env -> $"flow:{env}"))
            |> Flow.run "env"

        let asyncBridge =
            Check.okIf false
            |> AsyncFlow.orElseAsync (async.Return "async")
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let asyncFlowBridgeFromFlow =
            Check.okIf false
            |> AsyncFlow.orElseFlow (Flow.read (fun env -> $"async-flow:{env}"))
            |> AsyncFlow.run "env"
            |> Async.RunSynchronously

        let asyncFlowBridge =
            Check.okIf false
            |> AsyncFlow.orElseAsyncFlow (AsyncFlow.read (fun env -> $"async-flow:{env}"))
            |> AsyncFlow.run "env"
            |> Async.RunSynchronously

        let taskBridge =
            Check.okIf false
            |> TaskFlow.orElseTask (Task.FromResult "task")
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskAsyncBridge =
            Check.okIf false
            |> TaskFlow.orElseAsync (async.Return "task-async")
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskFlowBridge =
            Check.okIf false
            |> TaskFlow.orElseTaskFlow (TaskFlow.read (fun env -> $"task-flow:{env}"))
            |> TaskFlow.run "env" CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskAsyncFlowBridge =
            Check.okIf false
            |> TaskFlow.orElseAsyncFlow (AsyncFlow.read (fun env -> $"task-async-flow:{env}"))
            |> TaskFlow.run "env" CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let flowValue = Flow.value "flow-value" |> Flow.run ()
        let asyncValue = AsyncFlow.value "async-value" |> AsyncFlow.run () |> Async.RunSynchronously
        let taskValue =
            TaskFlow.value "task-value"
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        test <@ flowBridge = Exit.Failure (Cause.Fail "flow:env") @>
        test <@ asyncBridge = Exit.Failure (Cause.Fail "async") @>
        test <@ asyncFlowBridgeFromFlow = Exit.Failure (Cause.Fail "async-flow:env") @>
        test <@ asyncFlowBridge = Exit.Failure (Cause.Fail "async-flow:env") @>
        test <@ taskBridge = Exit.Failure (Cause.Fail "task") @>
        test <@ taskAsyncBridge = Exit.Failure (Cause.Fail "task-async") @>
        test <@ taskFlowBridge = Exit.Failure (Cause.Fail "task-flow:env") @>
        test <@ taskAsyncFlowBridge = Exit.Failure (Cause.Fail "task-async-flow:env") @>
        test <@ flowValue = Exit.Success "flow-value" @>
        test <@ asyncValue = Exit.Success "async-value" @>
        test <@ taskValue = Exit.Success "task-value" @>

    [<Fact>]
    let ``option and valueoption inputs short-circuit with unit errors across builders`` () =
        let syncSome : Flow<int, unit, int> =
            flow {
                let! env = Flow.env
                let! value = Some(env + 1)
                return value * 2
            }

        let syncNone : Flow<int, unit, int> =
            flow {
                let! env = Flow.env
                let! value = None
                return env + value
            }

        let syncValueSome : Flow<int, unit, int> =
            flow {
                let! env = Flow.env
                let! value = ValueSome(env + 1)
                return value * 2
            }

        let syncValueNone : Flow<int, unit, int> =
            flow {
                let! env = Flow.env
                let! value = ValueNone
                return env + value
            }

        let asyncWorkflow : Flow<int, unit, int> =
            flow {
                let! env = Flow.env
                let! value = Some(env + 1)
                let! extra = ValueSome(value + 1)
                return extra * 2
            }

        let asyncReturnFromNone : Flow<unit, unit, int> =
            flow { return! None }

        let taskWorkflow : Flow<int, unit, int> =
            flow {
                let! env = Flow.env
                let! value = Some(env + 1)
                let! extra = ValueSome(value + 1)
                return extra * 2
            }

        let taskReturnFromValueNone : Flow<unit, unit, int> =
            flow { return! ValueNone }

        let flowArgumentTypeNames = flowBuilderBindAndReturnFromArgumentNames ()

        test <@ Flow.run 20 syncSome = Exit.Success 42 @>
        test <@ Flow.run 20 syncNone = Exit.Failure (Cause.Fail ()) @>
        test <@ Flow.run 20 syncValueSome = Exit.Success 42 @>
        test <@ Flow.run 20 syncValueNone = Exit.Failure (Cause.Fail ()) @>
        test <@ Flow.run 19 asyncWorkflow = Exit.Success 42 @>
        test <@ Flow.run () asyncReturnFromNone = Exit.Failure (Cause.Fail ()) @>
        test <@ Flow.run 19 taskWorkflow = Exit.Success 42 @>
        test <@ Flow.run () taskReturnFromValueNone = Exit.Failure (Cause.Fail ()) @>
        test <@ flowArgumentTypeNames |> Array.contains "FSharpOption`1" @>
        test <@ flowArgumentTypeNames |> Array.contains "FSharpResult`2" @>
        test <@ flowArgumentTypeNames |> Array.contains "FSharpValueOption`1" @>

    [<Fact>]
    let ``option and valueoption implicit binding requires unit workflow errors`` () =
        let fsFlowAssemblyPath = typeof<FlowBuilder>.Assembly.Location
        let fsFlowNetAssemblyPath = typeof<TaskFlowBuilder>.Assembly.Location

        let flowProbe =
            $"""
#r @"{fsFlowAssemblyPath}"
open FsFlow

let probe : Flow<unit, string, int> =
    flow {{
        let! value = Some 42
        return value
    }}
"""

        let asyncProbe =
            $"""
#r @"{fsFlowAssemblyPath}"
open FsFlow

let probe : Flow<unit, string, int> =
    flow {{
        let! value = ValueSome 42
        return value
    }}
"""

        let taskProbe =
            $"""
#r @"{fsFlowAssemblyPath}"
#r @"{fsFlowNetAssemblyPath}"
open FsFlow

let probe : Flow<unit, string, int> =
    flow {{
        let! value = Some 42
        return value
    }}
"""

        let flowExitCode, flowOutput = runFsiScript flowProbe
        let asyncExitCode, asyncOutput = runFsiScript asyncProbe
        let taskExitCode, taskOutput = runFsiScript taskProbe

        test <@ flowExitCode <> 0 @>
        test <@ flowOutput.Contains("Flow<unit,unit,int>") @>
        test <@ asyncExitCode <> 0 @>
        test <@ asyncOutput.Contains("Flow<unit,unit,int>") @>
        test <@ taskExitCode <> 0 @>
        test <@ taskOutput.Contains("Flow<unit,unit,int>") @>

    [<Fact>]
    let ``explicit option adapters support custom workflow errors across modules`` () =
        let syncSome =
            Some 21
            |> Flow.fromOption "missing value"
            |> Flow.map ((*) 2)
            |> Flow.run ()

        let syncNone =
            None
            |> Flow.fromOption "missing value"
            |> Flow.run ()

        let syncValueSome =
            ValueSome 21
            |> Flow.fromValueOption "missing value"
            |> Flow.map ((*) 2)
            |> Flow.run ()

        let syncValueNone =
            ValueNone
            |> Flow.fromValueOption "missing value"
            |> Flow.run ()

        let asyncSome =
            Some 21
            |> AsyncFlow.fromOption "missing value"
            |> AsyncFlow.map ((*) 2)
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let asyncNone =
            None
            |> AsyncFlow.fromOption "missing value"
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let asyncValueSome =
            ValueSome 21
            |> AsyncFlow.fromValueOption "missing value"
            |> AsyncFlow.map ((*) 2)
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let asyncValueNone =
            ValueNone
            |> AsyncFlow.fromValueOption "missing value"
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        let taskSome =
            Some 21
            |> TaskFlow.fromOption "missing value"
            |> TaskFlow.map ((*) 2)
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskNone =
            None
            |> TaskFlow.fromOption "missing value"
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskValueSome =
            ValueSome 21
            |> TaskFlow.fromValueOption "missing value"
            |> TaskFlow.map ((*) 2)
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        let taskValueNone =
            ValueNone
            |> TaskFlow.fromValueOption "missing value"
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        test <@ syncSome = Exit.Success 42 @>
        test <@ syncNone = Exit.Failure (Cause.Fail "missing value") @>
        test <@ syncValueSome = Exit.Success 42 @>
        test <@ syncValueNone = Exit.Failure (Cause.Fail "missing value") @>
        test <@ asyncSome = Exit.Success 42 @>
        test <@ asyncNone = Exit.Failure (Cause.Fail "missing value") @>
        test <@ asyncValueSome = Exit.Success 42 @>
        test <@ asyncValueNone = Exit.Failure (Cause.Fail "missing value") @>
        test <@ taskSome = Exit.Success 42 @>
        test <@ taskNone = Exit.Failure (Cause.Fail "missing value") @>
        test <@ taskValueSome = Exit.Success 42 @>
        test <@ taskValueNone = Exit.Failure (Cause.Fail "missing value") @>

    [<Fact>]
    let ``Guard constructors work in all flow families`` () =
        let successOption : int option = Some 42
        let successValueOption : int voption = ValueSome 10
        let asyncOption : Async<int option> = async { return Some 42 }
        let asyncValueOption : Async<int voption> = async { return ValueSome 10 }
        let asyncBool : Async<bool> = async { return true }
        let successTaskOption : Task<int option> = Task.FromResult(Some 5)
        let successTaskValueOption : ValueTask<int voption> = ValueTask.FromResult(ValueSome 3)
        let guardedSuccessOption : Result<int, string> = Guard.Of("missing-option", successOption)
        let guardedSuccessValueOption : Result<int, string> = Guard.Of("missing-voption", successValueOption)
        let guardedBool : Result<unit, string> = Guard.Of("bool-false", true)
        let guardedAsyncOption : Async<Result<int, string>> = Guard.Of("missing-option", asyncOption)
        let guardedAsyncValueOption : Async<Result<int, string>> = Guard.Of("missing-voption", asyncValueOption)
        let guardedAsyncBool : Async<Result<unit, string>> = Guard.Of("bool-false", asyncBool)
        let guardedTaskOption : Task<Result<int, string>> = Guard.Of("task-missing", successTaskOption)
        let guardedTaskValueOption : ValueTask<Result<int, string>> = Guard.Of("vtask-missing", successTaskValueOption)

        let flowTest =
            flow {
                let! x = guardedSuccessOption
                let! y = guardedSuccessValueOption
                do! guardedBool
                return x + y
            }

        let asyncFlowTest =
            flow {
                let! (x : int) = guardedAsyncOption
                let! (y : int) = guardedAsyncValueOption
                do! guardedAsyncBool
                return x + y
            }

        let taskFlowTest =
            taskFlow {
                let! x = guardedSuccessOption
                let! y = guardedSuccessValueOption
                do! guardedBool
                let! z = guardedTaskOption
                let! w = guardedTaskValueOption
                return x + y + z + w
            }

        let flowResult = Flow.run () flowTest
        let asyncFlowResult = Flow.run () asyncFlowTest
        let taskFlowResult = TaskFlow.run () CancellationToken.None taskFlowTest |> fun t -> t.GetAwaiter().GetResult()

        test <@ flowResult = Exit.Success 52 @>
        test <@ asyncFlowResult = Exit.Success 52 @>
        test <@ taskFlowResult = Exit.Success 60 @>

    [<Fact>]
    let ``AsyncFlow login syntax uses Guard constructors and error mapping`` () =
        let tryGetUser username = async { return if username = "missing" then None else Some username }
        let isPwdValid password user = password = $"{user}-pwd"
        let authorize user = async { return if user = "blocked" then Error "denied" else Ok () }
        let createAuthToken user = if user = "expired" then Error "token-expired" else Ok $"token-{user}"

        let login username password =
            flow {
                let userResult : Async<Result<string, LoginError>> = Guard.Of(InvalidUser, tryGetUser username)
                let! (user : string) = userResult

                let passwordCheck : Result<unit, LoginError> = Guard.Of(InvalidPwd, isPwdValid password user)
                do! passwordCheck

                let authorizeResult : Async<Result<unit, LoginError>> = Guard.MapError(Unauthorized, authorize user)
                do! authorizeResult

                let tokenResult : Result<string, LoginError> = Guard.MapError(TokenErr, createAuthToken user)
                return! tokenResult
            }

        let success = Flow.run () (login "alice" "alice-pwd")
        let authFailure = Flow.run () (login "blocked" "blocked-pwd")
        let tokenFailure = Flow.run () (login "expired" "expired-pwd")

        test <@ success = Exit.Success "token-alice" @>
        test <@ authFailure = Exit.Failure (Cause.Fail (Unauthorized "denied")) @>
        test <@ tokenFailure = Exit.Failure (Cause.Fail (TokenErr "token-expired")) @>

    [<Fact>]
    let ``Guard mapError stays symmetric across flow families`` () =
        let asyncSource : Async<Result<int, string>> = async { return Error "async-source" }
        let taskSource : Task<Result<int, string>> = task { return Error "task-source" }
        let asyncSuccess : Async<Result<int, string>> = async { return Ok 1 }

        let asyncMapped =
            let mappedAsyncSource : Async<Result<int, string>> =
                Guard.MapError((fun error -> $"mapped-{error}"), asyncSource)

            flow {
                let! value = mappedAsyncSource
                return value + 1
            }

        let taskMapped =
            let mappedAsyncSuccess : Async<Result<int, string>> =
                Guard.MapError((fun error -> $"mapped-{error}"), asyncSuccess)

            let mappedTaskSource : Task<Result<int, string>> =
                Guard.MapError((fun error -> $"mapped-{error}"), taskSource)

            flow {
                let! (asyncValue : int) = mappedAsyncSuccess
                let! (taskValue : int) = mappedTaskSource
                return asyncValue + taskValue
            }

        test <@ Flow.run () asyncMapped = Exit.Failure (Cause.Fail "mapped-async-source") @>
        test <@ Flow.run () taskMapped = Exit.Failure (Cause.Fail "mapped-task-source") @>

    [<Fact>]
    let ``Guard.of fails correctly for check-like sources`` () =
        let missingOption : int option = None
        let guardedFlowFail : Result<int, string> = Guard.Of("failed", missingOption)
        let guardedAsyncFlowFail : Async<Result<int, string>> = Guard.Of("failed", async { return ValueNone })
        let guardedTaskFlowFail : Result<int, string> = Guard.Of("failed", missingOption)

        let flowFail = flow {
            let! (value : int) = guardedFlowFail
            return value
        }
        let asyncFlowFail = flow {
            let! (value : int) = guardedAsyncFlowFail
            return value
        }
        let taskFlowFail = flow {
            let! (value : int) = guardedTaskFlowFail
            return value
        }

        test <@ Flow.run () flowFail = Exit.Failure (Cause.Fail "failed") @>
        test <@ Flow.run () asyncFlowFail = Exit.Failure (Cause.Fail "failed") @>
        test <@ Flow.run () taskFlowFail = Exit.Failure (Cause.Fail "failed") @>
