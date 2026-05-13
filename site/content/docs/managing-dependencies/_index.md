---
weight: 40
title: Managing Dependencies
description: A staged guide to dependency management in FsFlow.
type: docs
---


FsFlow does not force one dependency style on every workflow. It gives you a ladder:

1. area-scoped records at the boundary, read with `Flow.read`
2. `RuntimeContext<'runtime, 'env>` when the host and app should be separate
3. `IServiceProvider` at the outer edge when the host container is the source of truth
4. nominal `Requires<'dep>` helpers when reuse beats plain record passing

Start with the shallowest level that fits the boundary you are designing. Read the record first, then split runtime from app dependencies only when that separation is real, then reach for provider lookup or nominal helpers only when the simpler shape starts to fight the design.

## Read In Order

| Level | Shape | Best fit | Main APIs |
| :--- | :--- | :--- | :--- |
| 1 | [Area-scoped records](./env-slicing/) | Controllers, jobs, integrations, feature boundaries | `Flow.env`, `Flow.read`, `Flow.localEnv`, `Flow.provideLayer` |
| 2 | [RuntimeContext<'runtime, 'env>](./runtime-context/) | Host services and app dependencies need separate ownership | `RuntimeContext.create`, `Resolver.runtime`, `Resolver.environment`, `Flow.readRuntime`, `Flow.readEnvironment` |
| 3 | [IServiceProvider edge](./provider-edge/) | ASP.NET, hosted services, DI-heavy hosts | `Resolver.fromProvider`, `MissingCapability` |
| 4 | [Nominal capability helpers](./capability-contracts/) | Reusable helpers that need a named contract | `Requires<'dep>`, `Resolver.resolve` |

## What `RuntimeContext` Is Doing Here

`RuntimeContext<'runtime, 'env>` is not a replacement for level 1. It is a different split:

- `Runtime` holds operational concerns such as logging, metrics, tracing, and clocks.
- `Environment` holds the application dependencies.
- `CancellationToken` belongs to the current task run.

That split matters when one host should feed many areas, or when the app record should stay focused while the runtime keeps expanding.

## Bridges, Not A Fifth Level

`Flow.localEnv` and `Flow.provideLayer` are bridges between shapes, not another dependency model.

- Use `localEnv` when a larger record can be projected to a smaller record.
- Use `provideLayer` when one flow builds the environment needed by another.
- Use `Resolver.resolve` when the workflow should read a dependency through the same vocabulary regardless of which level you are on.

```fsharp
let controllerWorkflow : Flow<ControllerDeps, Error, string> =
    flow {
        let! logger = Flow.read _.Logger
        logger.Info "starting"
        return "ok"
    }
```

The deeper levels exist to keep the boundary honest. They are not a license to make every workflow look the same.
