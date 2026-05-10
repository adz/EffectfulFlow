namespace FsFlow

open System
open System.Threading
open System.Threading.Tasks

type Flow<'env, 'error, 'value> with
#if FABLE_COMPILER
    static member inline CapabilityService
        (projection: 'env -> 'service)
        : Flow<'env, 'error, 'service> =
        Flow(fun environment _ -> EffectFlow.ofValue (projection environment))

    /// <summary>Reads a service from <see cref="IServiceProvider" /> and fails when it is not registered.</summary>
    static member inline ServiceFromProvider
        ()
        : Flow<IServiceProvider, MissingCapability, 'service> =
        Flow(fun provider _ ->
            match provider.GetService typeof<'service> with
            | null ->
                EffectFlow.ofError
                    {
                        CapabilityType = typeof<'service>
                    }
            | value -> EffectFlow.ofValue (unbox<'service> value))
#else
    static member CapabilityService
        (projection: 'env -> 'service)
        : Flow<'env, 'error, 'service> =
        Flow(fun environment _ -> EffectFlow.ofValue (projection environment))

    /// <summary>Reads a service from <see cref="IServiceProvider" /> and fails when it is not registered.</summary>
    static member ServiceFromProvider
        ()
        : Flow<IServiceProvider, MissingCapability, 'service> =
        Flow(fun provider _ ->
            match provider.GetService typeof<'service> with
            | null ->
                EffectFlow.ofError
                    {
                        CapabilityType = typeof<'service>
                    }
            | value -> EffectFlow.ofValue (unbox<'service> value))
#endif

    static member ProvideLayer
        (
            layer: Flow<'input, 'error, 'environment>,
            flow: Flow<'environment, 'error, 'value>
        ) : Flow<'input, 'error, 'value> =
        let (Flow layerOperation) = layer
        let (Flow flowOperation) = flow

        Flow(fun environment ct ->
            #if FABLE_COMPILER
            async {
                let! exit = layerOperation environment ct
                match exit with
                | Exit.Success environment -> return! flowOperation environment ct
                | Exit.Failure cause -> return Exit.Failure cause
            }
            #else
            match (layerOperation environment ct).GetAwaiter().GetResult() with
            | Exit.Success environment -> flowOperation environment ct
            | Exit.Failure cause -> EffectFlow.ofCause cause
            #endif
        )

#if !FABLE_COMPILER
type internal AsyncFlow<'env, 'error, 'value> with
    static member CapabilityService
        (projection: 'env -> 'service)
        : AsyncFlow<'env, 'error, 'service> =
        AsyncFlow(fun environment -> async.Return(Exit.Success(projection environment)))

    static member ServiceFromProvider
        ()
        : AsyncFlow<IServiceProvider, MissingCapability, 'service> =
        AsyncFlow(fun provider ->
            async {
                match provider.GetService typeof<'service> with
                | null ->
                    return
                        Exit.Failure (Cause.Fail
                            {
                                CapabilityType = typeof<'service>
                            })
                | value -> return Exit.Success(unbox<'service> value)
            })

    static member ProvideLayer
        (
            layer: AsyncFlow<'input, 'error, 'environment>,
            flow: AsyncFlow<'environment, 'error, 'value>
        ) : AsyncFlow<'input, 'error, 'value> =
        let (AsyncFlow layerOperation) = layer
        let (AsyncFlow flowOperation) = flow

        AsyncFlow(fun environment ->
            async {
                let! outcome = layerOperation environment

                match outcome with
                | Exit.Success environment -> return! flowOperation environment
                | Exit.Failure cause -> return Exit.Failure cause
            })

type internal TaskFlow<'env, 'error, 'value> with
    static member CapabilityService
        (projection: 'env -> 'service)
        : TaskFlow<'env, 'error, 'service> =
        TaskFlow(fun environment _ -> Task.FromResult(Exit.Success(projection environment)))

    static member ServiceFromProvider
        ()
        : TaskFlow<IServiceProvider, MissingCapability, 'service> =
        TaskFlow(fun provider _ ->
            match provider.GetService typeof<'service> with
            | null ->
                Task.FromResult(
                    Exit.Failure (Cause.Fail
                        {
                            CapabilityType = typeof<'service>
                        }))
            | value -> Task.FromResult(Exit.Success(unbox<'service> value)))

    static member ProvideLayer
        (
            layer: TaskFlow<'input, 'error, 'environment>,
            flow: TaskFlow<'environment, 'error, 'value>
        ) : TaskFlow<'input, 'error, 'value> =
        let (TaskFlow layerOperation) = layer
        let (TaskFlow flowOperation) = flow

        TaskFlow(fun environment cancellationToken ->
            task {
                let! outcome = layerOperation environment cancellationToken

                match outcome with
                | Exit.Success environment -> return! flowOperation environment cancellationToken
                | Exit.Failure cause -> return Exit.Failure cause
            })
#endif

/// <summary>Helpers for working with capabilities in task workflows.</summary>
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Capability =
    /// <summary>Reads a service from the environment using the provided projection.</summary>
    let service
        (projection: 'env -> 'service)
        : Flow<RuntimeContext<'runtime, 'env>, 'error, 'service> =
        Flow.read (fun context -> projection context.Environment)

    /// <summary>Reads the current runtime from the environment.</summary>
    let runtime : Flow<RuntimeContext<'runtime, 'env>, 'error, 'runtime> =
        Flow.read (fun context -> context.Runtime)

    /// <summary>Reads the application environment from the environment.</summary>
    let environment : Flow<RuntimeContext<'runtime, 'env>, 'error, 'env> =
        Flow.read (fun context -> context.Environment)

    /// <summary>Reads a service from <see cref="IServiceProvider" /> and fails when it is not registered.</summary>
    #if FABLE_COMPILER
    let inline serviceFromProvider<'service> : Flow<IServiceProvider, MissingCapability, 'service> =
        Flow.ServiceFromProvider()
    #else
    let serviceFromProvider<'service> : Flow<IServiceProvider, MissingCapability, 'service> =
        Flow.ServiceFromProvider()
    #endif

/// <summary>Helpers for deriving an environment in one flow and consuming it in another.</summary>
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Layer =
    /// <summary>Provides a derived environment from a layer flow to a downstream flow.</summary>
    let provideLayer
        (layer: Flow<'input, 'error, 'environment>)
        (flow: Flow<'environment, 'error, 'value>)
        : Flow<'input, 'error, 'value> =
        Flow.ProvideLayer(layer, flow)
