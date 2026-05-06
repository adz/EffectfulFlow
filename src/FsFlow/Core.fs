namespace FsFlow

open System
open System.Threading.Tasks

/// <summary>
/// Represents a cold synchronous workflow that reads an environment, returns a typed result,
/// and is executed explicitly through <c>Flow.run</c>.
/// </summary>
/// <typeparam name="env">The type of the environment dependency.</typeparam>
/// <typeparam name="error">The type of the failure value.</typeparam>
/// <typeparam name="value">The type of the success value.</typeparam>
type Flow<'env, 'error, 'value> =
    private
    | Flow of ('env -> Result<'value, 'error>)

/// <summary>
/// Represents a cold async workflow that reads an environment, returns a typed result,
/// and is executed explicitly through <c>AsyncFlow.run</c>.
/// </summary>
/// <typeparam name="env">The type of the environment dependency.</typeparam>
/// <typeparam name="error">The type of the failure value.</typeparam>
/// <typeparam name="value">The type of the success value.</typeparam>
type AsyncFlow<'env, 'error, 'value> =
    private
    | AsyncFlow of ('env -> Async<Result<'value, 'error>>)

/// <summary>
/// Log levels used by runtime logging helpers and environment-provided logging functions.
/// </summary>
[<RequireQualifiedAccess>]
type LogLevel =
    | Trace
    | Debug
    | Information
    | Warning
    | Error
    | Critical

/// <summary>
/// A structured log entry written through a runtime logger.
/// </summary>
type LogEntry =
    {
      Level: LogLevel
      Message: string
      TimestampUtc: DateTimeOffset
    }

/// <summary>
/// Defines how runtime retry helpers repeat typed failures in a controlled way.
/// </summary>
type RetryPolicy<'error> =
    {
      MaxAttempts: int
      Delay: int -> TimeSpan
      ShouldRetry: 'error -> bool
    }

/// <summary>
/// Standard retry policies for runtime helpers.
/// </summary>
[<RequireQualifiedAccess>]
module RetryPolicy =
    let noDelay (maxAttempts: int) : RetryPolicy<'error> =
        { MaxAttempts = maxAttempts
          Delay = fun _ -> TimeSpan.Zero
          ShouldRetry = fun _ -> true }

module private ResultFlow =
    let map
        (mapper: 'value -> 'next)
        (result: Result<'value, 'error>)
        : Result<'next, 'error> =
        Result.map mapper result

    let bind
        (binder: 'value -> Result<'next, 'error>)
        (result: Result<'value, 'error>)
        : Result<'next, 'error> =
        Result.bind binder result

    let mapError
        (mapper: 'error -> 'nextError)
        (result: Result<'value, 'error>)
        : Result<'value, 'nextError> =
        Result.mapError mapper result

module internal OptionFlow =
    let toUnitResult (value: 'value option) : Result<'value, unit> =
        match value with
        | Some innerValue -> Ok innerValue
        | None -> Error()

    let toUnitResultValueOption (value: 'value voption) : Result<'value, unit> =
        match value with
        | ValueSome innerValue -> Ok innerValue
        | ValueNone -> Error()

    let toResult (error: 'error) (value: 'value option) : Result<'value, 'error> =
        match value with
        | Some innerValue -> Ok innerValue
        | None -> Error error

    let toResultValueOption (error: 'error) (value: 'value voption) : Result<'value, 'error> =
        match value with
        | ValueSome innerValue -> Ok innerValue
        | ValueNone -> Error error

module internal InternalCombinatorCore =
    let mapWith
        (mapOutcome: (Result<'value, 'error> -> Result<'next, 'error>) -> 'operation -> 'nextOperation)
        (mapper: 'value -> 'next)
        (operation: 'context -> 'operation)
        : 'context -> 'nextOperation =
        fun context -> operation context |> mapOutcome (Result.map mapper)

    let bindWith
        (bindOutcome: 'operation -> ('value -> 'nextOperation) -> ('error -> 'nextOperation) -> 'nextOperation)
        (continueWith: 'context -> 'value -> 'nextOperation)
        (failWith: 'error -> 'nextOperation)
        (operation: 'context -> 'operation)
        : 'context -> 'nextOperation =
        fun context -> bindOutcome (operation context) (continueWith context) failWith

    let mapErrorWith
        (mapOutcome: (Result<'value, 'error> -> Result<'value, 'nextError>) -> 'operation -> 'nextOperation)
        (mapper: 'error -> 'nextError)
        (operation: 'context -> 'operation)
        : 'context -> 'nextOperation =
        fun context -> operation context |> mapOutcome (Result.mapError mapper)

    let localEnvWith
        (run: 'innerEnvironment -> 'flow -> 'operation)
        (mapping: 'outerEnvironment -> 'innerEnvironment)
        (flow: 'flow)
        : 'outerEnvironment -> 'operation =
        fun environment -> flow |> run (mapping environment)

    let delayWith
        (run: 'environment -> 'flow -> 'operation)
        (factory: unit -> 'flow)
        : 'environment -> 'operation =
        fun environment -> factory () |> run environment
