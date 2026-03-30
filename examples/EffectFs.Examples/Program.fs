open System
open System.Threading
open System.Threading.Tasks
open EffectFs

type AppConfig =
    { ApiBaseUrl: string
      ApiKey: string
      RetryCount: int
      RequestTimeout: TimeSpan
      NetworkDelay: TimeSpan
      Prefix: string
      FailuresBeforeSuccess: int
      SimulateLegacyFailure: bool }

type ValidationError =
    | MissingValue of string
    | NonPositiveNumber of string

type GatewayError =
    | Transient of int
    | Rejected of string

type AppError =
    | ValidationFailed of ValidationError
    | GatewayFailed of GatewayError
    | PersistenceFailed of string
    | LegacyFailure of string
    | TimedOut
    | Canceled

type RequestPlan =
    { Banner: string
      Url: string
      RetryCount: int
      RequestTimeout: TimeSpan
      NetworkDelay: TimeSpan }

type Response =
    { StatusCode: int
      Body: string }

type AuditRecord =
    { Url: string
      Attempts: int
      StatusCode: int }

type IPingGateway =
    abstract Ping: RequestPlan * CancellationToken -> Task<Result<Response, GatewayError>>

type IAuditStore =
    abstract Save: AuditRecord * CancellationToken -> Task
    abstract Snapshot: unit -> AuditRecord list

type AppEnvironment =
    { Config: AppConfig
      Gateway: IPingGateway
      AuditStore: IAuditStore
      AttemptCount: int ref
      WriteLog: LogEntry -> unit }

type RequestScope(writeLog: LogEntry -> unit, name: string) =
    do
        writeLog
            { Level = LogLevel.Debug
              Message = sprintf "opening scope=%s" name
              TimestampUtc = DateTimeOffset.UtcNow }

    interface IAsyncDisposable with
        member _.DisposeAsync() =
            writeLog
                { Level = LogLevel.Debug
                  Message = sprintf "closing scope=%s" name
                  TimestampUtc = DateTimeOffset.UtcNow }

            ValueTask()

type FakePingGateway(attemptCount: int ref, config: AppConfig) =
    interface IPingGateway with
        member _.Ping(plan: RequestPlan, cancellationToken: CancellationToken) =
            task {
                do! Task.Delay(plan.NetworkDelay, cancellationToken)

                let attempt = attemptCount.Value + 1
                attemptCount.Value <- attempt

                if plan.Url.Contains("blocked") then
                    return Error(Rejected "upstream rejected the request")
                elif attempt <= config.FailuresBeforeSuccess then
                    return Error(Transient attempt)
                else
                    return
                        Ok
                            { StatusCode = 200
                              Body = sprintf "GET %s with key=%s" plan.Url config.ApiKey }
            }

type InMemoryAuditStore(writeLog: LogEntry -> unit) =
    let records = ResizeArray<AuditRecord>()

    interface IAuditStore with
        member _.Save(record: AuditRecord, cancellationToken: CancellationToken) =
            task {
                cancellationToken.ThrowIfCancellationRequested()
                do! Task.Delay(5, cancellationToken)
                records.Add record
                writeLog
                    { Level = LogLevel.Information
                      Message = sprintf "audit saved url=%s attempts=%d" record.Url record.Attempts
                      TimestampUtc = DateTimeOffset.UtcNow }
            }

        member _.Snapshot() = List.ofSeq records

let execute<'env, 'value>
    (environment: 'env)
    (workflow: Effect<'env, AppError, 'value>)
    : Result<'value, AppError> =
    workflow
    |> Effect.execute environment
    |> Async.RunSynchronously

let requireNonEmpty (name: string) (value: string) : Result<string, ValidationError> =
    if String.IsNullOrWhiteSpace value then
        Error(MissingValue name)
    else
        Ok value

let requirePositiveDurationMs (name: string) (value: TimeSpan) : Result<TimeSpan, ValidationError> =
    if value > TimeSpan.Zero then
        Ok value
    else
        Error(NonPositiveNumber name)

let requirePositiveInt (name: string) (value: int) : Result<int, ValidationError> =
    if value > 0 then
        Ok value
    else
        Error(NonPositiveNumber name)

let logWith level messageFactory : Effect<AppEnvironment, AppError, unit> =
    Effect.logWith (fun env entry -> env.WriteLog entry) level messageFactory

let logInfo message : Effect<AppEnvironment, AppError, unit> =
    logWith LogLevel.Information (fun _ -> message)

let createEnvironment (config: AppConfig) : AppEnvironment =
    let attemptCount = ref 0

    let writeLog entry =
        printfn "[%A] %s" entry.Level entry.Message

    { Config = config
      Gateway = FakePingGateway(attemptCount, config) :> IPingGateway
      AuditStore = InMemoryAuditStore(writeLog) :> IAuditStore
      AttemptCount = attemptCount
      WriteLog = writeLog }

let validateConfig : Effect<AppConfig, AppError, RequestPlan> =
    effect {
        let! config = Effect.environment<AppConfig, AppError>

        let! apiBaseUrl =
            requireNonEmpty "apiBaseUrl" config.ApiBaseUrl
            |> Result.mapError ValidationFailed

        let! _ =
            requireNonEmpty "apiKey" config.ApiKey
            |> Result.mapError ValidationFailed

        let! retryCount =
            requirePositiveInt "retryCount" config.RetryCount
            |> Result.mapError ValidationFailed

        let! requestTimeout =
            requirePositiveDurationMs "requestTimeout" config.RequestTimeout
            |> Result.mapError ValidationFailed

        let! networkDelay =
            requirePositiveDurationMs "networkDelay" config.NetworkDelay
            |> Result.mapError ValidationFailed

        return
            { Banner =
                sprintf "%s :: %s" config.Prefix config.ApiKey
                |> fun value -> value.ToUpperInvariant()
              Url = sprintf "%s/ping" apiBaseUrl
              RetryCount = retryCount
              RequestTimeout = requestTimeout
              NetworkDelay = networkDelay }
    }

let fetchResponse (plan: RequestPlan) : Effect<AppEnvironment, AppError, Response> =
    let invokeGateway =
        effect {
            let! environment = Effect.environment<AppEnvironment, AppError>
            let! token = Effect.cancellationToken<AppEnvironment, AppError>
            do! logWith LogLevel.Information (fun env -> sprintf "gateway call attempt=%d url=%s" (env.AttemptCount.Value + 1) plan.Url)

            let! response =
                environment.Gateway.Ping(plan, token)
                |> Effect.fromTaskResultValue
                |> Effect.mapError GatewayFailed

            return response
        }

    invokeGateway
    |> Effect.retry
        { MaxAttempts = plan.RetryCount + 1
          Delay = fun attempt -> TimeSpan.FromMilliseconds(float (attempt * 15))
          ShouldRetry =
            function
            | GatewayFailed(Transient _) -> true
            | _ -> false }
    |> Effect.timeout plan.RequestTimeout TimedOut
    |> Effect.catchCancellation (fun _ -> Canceled)

let saveAudit (plan: RequestPlan) (response: Response) : Effect<AppEnvironment, AppError, unit> =
    effect {
        let! environment = Effect.environment<AppEnvironment, AppError>
        let record =
            { Url = plan.Url
              Attempts = environment.AttemptCount.Value
              StatusCode = response.StatusCode }

        let! token = Effect.cancellationToken<AppEnvironment, AppError>

        do!
            environment.AuditStore.Save(record, token)
            |> Effect.fromTaskUnit
            |> Effect.catch (fun error -> PersistenceFailed error.Message)
    }

let runLegacyBoundary : Effect<AppConfig, AppError, unit> =
    Effect.delay(fun () ->
        effect {
            let! shouldFail = Effect.read (fun (config: AppConfig) -> config.SimulateLegacyFailure)

            if shouldFail then
                invalidOp "legacy logger exploded"

            return ()
        })
    |> Effect.catch (fun error -> LegacyFailure error.Message)

let program : Effect<AppConfig, AppError, string> =
    effect {
        let! config = Effect.environment<AppConfig, AppError>
        let environment = createEnvironment config

        let runInAppEnvironment workflow =
            workflow |> Effect.withEnvironment (fun (_: AppConfig) -> environment)

        let! plan = validateConfig

        let! response =
            Effect.usingAsync
                (RequestScope(environment.WriteLog, "request"))
                (fun _ ->
                    effect {
                        do! runInAppEnvironment (logInfo "starting request workflow")
                        let! response = runInAppEnvironment (fetchResponse plan)
                        do! runInAppEnvironment (saveAudit plan response)
                        return response
                    })

        let! () = runLegacyBoundary

        let audits =
            environment.AuditStore.Snapshot()
            |> List.length

        return
            sprintf
                "%s -> %d %s (attempts=%d audits=%d)"
                plan.Banner
                response.StatusCode
                response.Body
                environment.AttemptCount.Value
                audits
    }

let printScenario (label: string) (config: AppConfig) : unit =
    printfn ""
    printfn "== %s ==" label
    printfn "input: %A" config

    let result = execute config program
    printfn "result: %A" result

[<EntryPoint>]
let main _ =
    let success =
        { ApiBaseUrl = "https://example.test"
          ApiKey = "demo-key"
          RetryCount = 2
          RequestTimeout = TimeSpan.FromMilliseconds 200
          NetworkDelay = TimeSpan.FromMilliseconds 25
          Prefix = "demo"
          FailuresBeforeSuccess = 1
          SimulateLegacyFailure = false }

    let validationFailure =
        { success with
            ApiKey = ""
            RetryCount = 0
            RequestTimeout = TimeSpan.Zero }

    let retriesExhausted =
        { success with
            RetryCount = 1
            FailuresBeforeSuccess = 2 }

    let timedOut =
        { success with
            RequestTimeout = TimeSpan.FromMilliseconds 10
            NetworkDelay = TimeSpan.FromMilliseconds 50 }

    let legacyFailure =
        { success with
            SimulateLegacyFailure = true }

    printScenario "Success" success
    printScenario "Validation Failure" validationFailure
    printScenario "Retries Exhausted" retriesExhausted
    printScenario "Timeout" timedOut
    printScenario "Legacy Failure Boundary" legacyFailure
    0
