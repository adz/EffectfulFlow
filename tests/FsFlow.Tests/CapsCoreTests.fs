namespace FsFlow.Tests

open System
open FsFlow
open FsFlow.Capabilities.Core
open Swensen.Unquote
open Xunit

module CapsCoreTests =
    [<Fact>]
    let ``clock random guid and environment variable helpers are deterministic when fixed`` () =
        let clock = Clock.fromValue (DateTimeOffset(2026, 5, 10, 12, 0, 0, TimeSpan.Zero))
        let random = Random.fromValue 4
        let guid = Guid.fromValue (global.System.Guid.Parse "11111111-1111-1111-1111-111111111111")
        let envVars =
            EnvironmentVariables.fromPairs
                [ "FSFLOW_CAPS_PORT", "8080"
                  "FSFLOW_CAPS_ENABLED", "true"
                  "FSFLOW_CAPS_SESSION", "22222222-2222-2222-2222-222222222222" ]

        test <@ Flow.runSync () (Clock.now |> Flow.withClock clock) = Exit.Success (DateTimeOffset(2026, 5, 10, 12, 0, 0, TimeSpan.Zero)) @>
        test <@ Flow.runSync () (Random.nextInt 0 10 |> Flow.withRandom random) = Exit.Success 4 @>
        test <@ Flow.runSync () (Guid.newGuid |> Flow.withGuid guid) = Exit.Success (global.System.Guid.Parse "11111111-1111-1111-1111-111111111111") @>
        test <@ Flow.runSync () (EnvironmentVariables.tryGet "FSFLOW_CAPS_PORT" |> Flow.withEnvironmentVariables envVars) = Exit.Success (Some "8080") @>
        test <@ Flow.runSync () (EnvironmentVariable.get "FSFLOW_CAPS_PORT" |> Flow.withEnvironmentVariables envVars) = Exit.Success "8080" @>
        test <@ Flow.runSync () (EnvironmentVariable.getInt "FSFLOW_CAPS_PORT" |> Flow.withEnvironmentVariables envVars) = Exit.Success 8080 @>
        test <@ Flow.runSync () (EnvironmentVariable.getBool "FSFLOW_CAPS_ENABLED" |> Flow.withEnvironmentVariables envVars) = Exit.Success true @>
        test <@ Flow.runSync () (EnvironmentVariable.getGuid "FSFLOW_CAPS_SESSION" |> Flow.withEnvironmentVariables envVars) = Exit.Success (global.System.Guid.Parse "22222222-2222-2222-2222-222222222222") @>

    [<Fact>]
    let ``environment variable helpers return typed errors for missing and invalid values`` () =
        let envVars =
            EnvironmentVariables.fromPairs
                [ "FSFLOW_CAPS_PORT_TEXT", "abc"
                  "FSFLOW_CAPS_ENABLED_TEXT", "maybe" ]

        test <@ Flow.runSync () (EnvironmentVariable.get "FSFLOW_CAPS_MISSING" |> Flow.withEnvironmentVariables envVars) = Exit.Failure(Cause.Fail (EnvironmentVariableError.MissingVariable "FSFLOW_CAPS_MISSING")) @>
        test <@ Flow.runSync () (EnvironmentVariable.getInt "FSFLOW_CAPS_PORT_TEXT" |> Flow.withEnvironmentVariables envVars) = Exit.Failure(Cause.Fail (EnvironmentVariableError.InvalidVariable("FSFLOW_CAPS_PORT_TEXT", "abc", "an integer"))) @>
        test <@ Flow.runSync () (EnvironmentVariable.getBool "FSFLOW_CAPS_ENABLED_TEXT" |> Flow.withEnvironmentVariables envVars) = Exit.Failure(Cause.Fail (EnvironmentVariableError.InvalidVariable("FSFLOW_CAPS_ENABLED_TEXT", "maybe", "a boolean"))) @>

    [<Fact>]
    let ``live providers work correctly with real runtime`` () =
        let timestamp = Flow.runSync () Clock.now |> function Exit.Success t -> t | _ -> failwith "Failed"
        let randomValue = Flow.runSync () (Random.nextInt 0 10) |> function Exit.Success v -> v | _ -> failwith "Failed"
        let generatedGuid = Flow.runSync () Guid.newGuid |> function Exit.Success g -> g | _ -> failwith "Failed"
        
        let envName = $"FSFLOW_CAPS_CORE_{global.System.Guid.NewGuid():N}"
        let previous = Environment.GetEnvironmentVariable envName

        try
            Environment.SetEnvironmentVariable(envName, "live-value")

            test <@ timestamp <= DateTimeOffset.UtcNow.AddMinutes(1.0) @>
            test <@ randomValue >= 0 && randomValue < 10 @>
            test <@ generatedGuid <> global.System.Guid.Empty @>
            test <@ Flow.runSync () (EnvironmentVariable.get envName) = Exit.Success "live-value" @>
        finally
            Environment.SetEnvironmentVariable(envName, previous)
