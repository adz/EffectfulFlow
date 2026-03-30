namespace EffectFs

open System
open System.Threading
open System.Threading.Tasks

type Effect<'env, 'error, 'value> =
    private
    | Effect of ('env -> CancellationToken -> Async<Result<'value, 'error>>)

[<RequireQualifiedAccess>]
module Effect =
    let run
        (environment: 'env)
        (cancellationToken: CancellationToken)
        (Effect operation: Effect<'env, 'error, 'value>)
        : Async<Result<'value, 'error>> =
        operation environment cancellationToken

    let succeed (value: 'value) : Effect<'env, 'error, 'value> =
        Effect(fun _ _ -> async.Return(Ok value))

    let fail (error: 'error) : Effect<'env, 'error, 'value> =
        Effect(fun _ _ -> async.Return(Error error))

    let ofResult (result: Result<'value, 'error>) : Effect<'env, 'error, 'value> =
        Effect(fun _ _ -> async.Return result)

    let ofAsync (operation: Async<'value>) : Effect<'env, 'error, 'value> =
        Effect(fun _ _ ->
            async {
                let! value = operation
                return Ok value
            })

    let ofTask (factory: CancellationToken -> Task<'value>) : Effect<'env, 'error, 'value> =
        Effect(fun _ cancellationToken ->
            async {
                let! value = factory cancellationToken |> Async.AwaitTask
                return Ok value
            })

    let ask<'env, 'error> : Effect<'env, 'error, 'env> =
        Effect(fun environment _ -> async.Return(Ok environment))

    let read (projection: 'env -> 'value) : Effect<'env, 'error, 'value> =
        Effect(fun environment _ -> async.Return(Ok(projection environment)))

    let map
        (mapper: 'value -> 'next)
        (effect: Effect<'env, 'error, 'value>)
        : Effect<'env, 'error, 'next> =
        Effect(fun environment cancellationToken ->
            async {
                let! result = run environment cancellationToken effect
                return Result.map mapper result
            })

    let bind
        (binder: 'value -> Effect<'env, 'error, 'next>)
        (effect: Effect<'env, 'error, 'value>)
        : Effect<'env, 'error, 'next> =
        Effect(fun environment cancellationToken ->
            async {
                let! result = run environment cancellationToken effect

                match result with
                | Ok value -> return! run environment cancellationToken (binder value)
                | Error error -> return Error error
            })

    let mapError
        (mapper: 'error -> 'nextError)
        (effect: Effect<'env, 'error, 'value>)
        : Effect<'env, 'nextError, 'value> =
        Effect(fun environment cancellationToken ->
            async {
                let! result = run environment cancellationToken effect
                return Result.mapError mapper result
            })

    let catch
        (handler: exn -> 'error)
        (effect: Effect<'env, 'error, 'value>)
        : Effect<'env, 'error, 'value> =
        Effect(fun environment cancellationToken ->
            async {
                try
                    return! run environment cancellationToken effect
                with error ->
                    return Error(handler error)
            })

    let delay (factory: unit -> Effect<'env, 'error, 'value>) : Effect<'env, 'error, 'value> =
        Effect(fun environment cancellationToken ->
            run environment cancellationToken (factory ()))

    let execute (environment: 'env) (effect: Effect<'env, 'error, 'value>) : Async<Result<'value, 'error>> =
        run environment CancellationToken.None effect

type EffectBuilder() =
    member _.Return(value: 'value) : Effect<'env, 'error, 'value> =
        Effect.succeed value

    member _.ReturnFrom(effect: Effect<'env, 'error, 'value>) : Effect<'env, 'error, 'value> =
        effect

    member _.Bind
        (
            effect: Effect<'env, 'error, 'value>,
            binder: 'value -> Effect<'env, 'error, 'next>
        ) : Effect<'env, 'error, 'next> =
        Effect.bind binder effect

    member _.Zero() : Effect<'env, 'error, unit> =
        Effect.succeed ()

    member _.Delay(factory: unit -> Effect<'env, 'error, 'value>) : Effect<'env, 'error, 'value> =
        Effect.delay factory

    member _.Combine
        (
            left: Effect<'env, 'error, unit>,
            right: Effect<'env, 'error, 'value>
        ) : Effect<'env, 'error, 'value> =
        Effect.bind (fun () -> right) left

    member _.TryWith
        (
            effect: Effect<'env, 'error, 'value>,
            handler: exn -> 'error
        ) : Effect<'env, 'error, 'value> =
        Effect.catch handler effect

[<AutoOpen>]
module EffectBuilderModule =
    let effect = EffectBuilder()
