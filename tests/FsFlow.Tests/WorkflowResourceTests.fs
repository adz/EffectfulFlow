namespace FsFlow.Tests

open System
open System.IO
open System.Threading
open System.Threading.Tasks
open FsFlow
open FsFlow.Tests.TestSupport
open Swensen.Unquote
open Xunit

module WorkflowResourceTests =
    [<Fact>]
    let ``AsyncFlow runtime helpers cover release`` () =
        let releaseCount = ref 0

        let acquireReleaseResult =
            AsyncFlow.Runtime.useWithAcquireRelease
                (AsyncFlow.succeed 7)
                (fun _ _ ->
                    releaseCount.Value <- releaseCount.Value + 1
                    Task.CompletedTask)
                (fun _ -> AsyncFlow.fail "boom")
            |> AsyncFlow.run ()
            |> Async.RunSynchronously

        test <@ acquireReleaseResult = Exit.Failure (Cause.Fail "boom") @>
        test <@ releaseCount.Value = 1 @>

    [<Fact>]
    let ``TaskFlow runtime helpers cover release`` () =
        let releaseCount = ref 0

        let acquireReleaseResult =
            TaskFlow.Runtime.useWithAcquireRelease
                (TaskFlow.succeed 7)
                (fun _ _ ->
                    releaseCount.Value <- releaseCount.Value + 1
                    Task.CompletedTask)
                (fun _ -> TaskFlow.fail "boom")
            |> TaskFlow.run () CancellationToken.None
            |> fun task -> task.GetAwaiter().GetResult()

        test <@ acquireReleaseResult = Exit.Failure (Cause.Fail "boom") @>
        test <@ releaseCount.Value = 1 @>
