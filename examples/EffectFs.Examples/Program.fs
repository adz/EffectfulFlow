open System
open System.Threading
open System.Threading.Tasks
open EffectFs

// Getting started:
//
// - use plain F# for pure validation and transformation
// - use Effect when code needs configuration, async work, tasks, or typed failures
// - keep the effect boundary visible in the type

type AppConfig =
    { ApiBaseUrl: string
      ApiKey: string
      RetryCount: int
      Prefix: string
      SimulateLegacyFailure: bool }

type ValidationError =
    | MissingValue of string
    | NonPositiveNumber of string

type AppError =
    | ValidationFailed of ValidationError
    | LegacyFailure of string

type RequestPlan =
    { Banner: string
      Url: string
      RetryCount: int }

type Response =
    { StatusCode: int
      Body: string }

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

let requirePositive (name: string) (value: int) : Result<int, ValidationError> =
    if value > 0 then
        Ok value
    else
        Error(NonPositiveNumber name)

let validateConfig : Effect<AppConfig, AppError, RequestPlan> =
    effect {
        let! config = Effect.ask<AppConfig, AppError>

        let! apiBaseUrl =
            config.ApiBaseUrl
            |> requireNonEmpty "apiBaseUrl"
            |> Result.mapError ValidationFailed
            |> Effect.ofResult

        let! apiKey =
            config.ApiKey
            |> requireNonEmpty "apiKey"
            |> Result.mapError ValidationFailed
            |> Effect.ofResult

        let! retryCount =
            config.RetryCount
            |> requirePositive "retryCount"
            |> Result.mapError ValidationFailed
            |> Effect.ofResult

        let banner =
            sprintf "%s :: %s" config.Prefix apiKey
            |> fun value -> value.ToUpperInvariant()

        return
            { Banner = banner
              Url = sprintf "%s/ping" apiBaseUrl
              RetryCount = retryCount }
    }

let fetchResponse (plan: RequestPlan) : Effect<AppConfig, AppError, Response> =
    Effect.ofTask(fun (_: CancellationToken) ->
        Task.FromResult
            { StatusCode = 200
              Body = sprintf "GET %s (retries=%d)" plan.Url plan.RetryCount })

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
        let! plan = validateConfig
        let! response = fetchResponse plan
        let! () = runLegacyBoundary

        return sprintf "%s -> %d %s" plan.Banner response.StatusCode response.Body
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
          Prefix = "demo"
          SimulateLegacyFailure = false }

    let validationFailure =
        { success with
            ApiKey = ""
            RetryCount = 0 }

    let legacyFailure =
        { success with
            SimulateLegacyFailure = true }

    printScenario "Success" success
    printScenario "Validation Failure" validationFailure
    printScenario "Legacy Failure Boundary" legacyFailure
    0
