---
weight: 30
title: "RuntimeContext"
description: Splitting runtime services from application dependencies.
---

# RuntimeContext

`RuntimeContext<'runtime, 'env>` gives your app services a separate lane from the host runtime.

This gives you:

- logging, metrics, tracing, or clocking belong to the host runtime
- your app services belong to a feature module or boundary record
- the same runtime services should be shared across multiple areas
- the cancellation token should travel with the execution model

`RuntimeContext` is the execution carrier above the adapter layer. It is not the runtime storage
engine and it is not the only way to model a dependency boundary.

## What Goes Where

- `Runtime` holds host runtime services such as logging, metrics, tracing, and clocks.
- `Environment` holds your app services.
- `CancellationToken` belongs to the active run.

```fsharp
type RuntimeServices =
    { Log : LogEntry -> unit
      Clock : IClock }

type ApiDeps =
    { Orders : IOrderRepository
      Email : IEmailSender }

let context =
    RuntimeContext.create runtime apiDeps cancellationToken
```

## Reading The Split

`Flow.readRuntime` and `Flow.readEnvironment` read the two halves.
`Resolver.runtime` and `Resolver.environment` read the same split at the host edge.

```fsharp
let workflow : Flow<RuntimeContext<RuntimeServices, ApiDeps>, string, Guid> =
    flow {
        let! runtime = Flow.readRuntime id
        let! app = Flow.readEnvironment id

        runtime.Log { Level = LogLevel.Information; Message = "starting"; TimestampUtc = runtime.Clock.UtcNow() }

        let! order = app.Orders.Create()
        do! app.Email.SendConfirmation order
        return order.Id
    }
```

Here, `runtime` is the host runtime and `app` is the app service set the workflow actually uses.

## What Works With RuntimeContext

Works with any environment:

- `Flow.env`
- `Flow.read`
- `Flow.localEnv`
- `Flow.provideLayer`
- `Resolver.resolve`
- `Resolver.fromProvider`
- `Flow.readRuntime`
- `Flow.readEnvironment`

RuntimeContext-specific:

- `RuntimeContext.create`
- `RuntimeContext.runtime`
- `RuntimeContext.environment`
- `RuntimeContext.cancellationToken`
- `Flow.readRuntime`
- `Flow.readEnvironment`
- `Resolver.runtime`
- `Resolver.environment`

The general helpers work on any environment, while the runtime split helpers only make sense when
the environment is actually a `RuntimeContext`.

## When To Stop

If the split only exists because it sounds cleaner, stop and use a concrete record.

`RuntimeContext` is worth using when your app services need a separate lane from the host runtime
and you want to read them through `Flow.readRuntime` and `Flow.readEnvironment`.

Keep the adapter layer at the boundary that creates the `RuntimeContext`; do not thread it through
every helper just because it is available.

See the [RuntimeContext reference](../../reference/runtime/) for the constructors and mapping
helpers, and the [Capability reference](../../reference/capability/) for the `runtime`,
`environment`, and `resolve` readers.
