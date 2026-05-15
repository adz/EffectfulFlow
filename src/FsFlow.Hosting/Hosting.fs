namespace FsFlow.Hosting

open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open FsFlow
open FsFlow.Capabilities.Core

/// <summary>
/// A live clock implementation that uses <see cref="P:System.DateTimeOffset.UtcNow" />.
/// </summary>
type LiveClock() =
    interface IClock with
        member _.UtcNow() = DateTimeOffset.UtcNow

/// <summary>A standard runtime that carries common operational services.</summary>
type DefaultRuntime =
    {
        Clock: IClock
        Log: ILog
    }

[<RequireQualifiedAccess>]
module Hosting =
    /// <summary>Creates a default runtime from an <see cref="T:System.IServiceProvider" />.</summary>
    let createRuntime (sp: IServiceProvider) : DefaultRuntime =
        let loggerFactory = sp.GetRequiredService<ILoggerFactory>()
        let logger = loggerFactory.CreateLogger("FsFlow")

        {
            Clock = LiveClock()
            Log = { new ILog with member _.Info message = logger.LogInformation message }
        }

    /// <summary>Executes a flow using services from the provided <see cref="T:System.IServiceProvider" />.</summary>
    let run (sp: IServiceProvider) (appEnv: 'appEnv) (flow: Flow<'appEnv, 'error, 'value>) : Effect<'value, 'error> =
        let runtime = createRuntime sp
        flow
        |> Flow.withClock runtime.Clock
        |> Flow.withLog runtime.Log
        |> Flow.run appEnv

[<RequireQualifiedAccess>]
module Startup =
    /// <summary>Validates that all required environment variables are present and valid using the ambient runtime.</summary>
    let validateEnvironment (flow: Flow<unit, EnvironmentVariableError, 'v>) : Result<'v, string list> =
        match (Flow.run () flow).AsTask().GetAwaiter().GetResult() with
        | Exit.Success v -> Ok v
        | Exit.Failure (Cause.Fail e) -> Error [ EnvironmentVariableErrors.describe e ]
        | Exit.Failure Cause.Interrupt -> Error [ "Validation was interrupted" ]
        | Exit.Failure (Cause.Die ex) -> Error [ ex.Message ]
