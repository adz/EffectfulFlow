---
weight: 40
title: Managing Dependencies
description: How FsFlow models dependency boundaries after the Registry / Scope / Layer foundation.
type: docs
---


In FsFlow, `env` can be any type you want. Within a `flow {}`, `Flow.env` gives you the whole
environment value, and `Flow.read` projects the piece you need from it. That is the basic shape of
dependency handling in FsFlow: keep the environment explicit, and read only what the flow actually
needs.

> **Tutorials Available:** For step-by-step guides on setting up these patterns, see the [Tutorials section](../../tutorials/).

In practice, that `env` often becomes a record. Records are easy to construct, easy to pass around,
and easy to understand at the boundary of an application.

FsFlow also supports these shapes:

- a nominal capability puts a name on the app dependency surface so the compiler can check it and
  helper signatures can advertise it
- runtime-owned services like clock, logging, random, and guid are implicit in the runtime, stay
  outside the visible environment, and can be overridden with `Flow.withClock`, `Flow.withLog`,
  `Flow.withRandom`, and `Flow.withGuid`
- standard `.NET` AppHost plus DI adapts the host container into a boundary record or nominal
  contract once, then workflow code stays on typed values

These are different shapes for different dependency boundaries. None of them is “more real” than
the others.

## Read In Order

| Step | Shape | When to use | Tutorial | Main APIs |
| :--- | :--- | :--- | :--- | :--- |
| 1 | Environment + record | One record gives you an explicit boundary, direct field access, and easy construction | **[AppRecord](../../tutorials/app-record/)** | `Flow.env`, `Flow.read`, `Flow.localEnv` |
| 2 | [Nominal capability contracts](./capability-contracts/) | An interface through a capability contract puts the dependency surface in the type, so the compiler checks it and helpers advertise it | **[Capabilities](../../tutorials/capabilities/)** | interfaces, `Flow.read` |
| 3 | Runtime overrides | Operational services stay runtime-owned and are supplied at execution time | **[AppHost](../../tutorials/app-host/)** | `Flow.withClock`, `Flow.withLog`, `Flow.withRandom`, `Flow.withGuid` |
| 4 | [Standard .NET AppHost Plus DI](./provider-edge/) | Host registrations become a boundary record or nominal contract once, and workflow code stays on typed values | **[AppHost](../../tutorials/app-host/)** | `Resolver.fromProvider`, `MissingCapability` |

## Start With A Record

Records keep the boundary concrete. They are easy to read, easy to construct, and easy to pass
around.

```fsharp
type ApiDeps =
    { Orders : IOrderRepository
      Email : IEmailSender
      Clock : IClock }

let workflow : Flow<ApiDeps, string, OrderId> =
    flow {
        let! deps = Flow.env
        let! email = Flow.read _.Email
        let! created = deps.Orders.Create()
        do! email.SendConfirmation created
        return created.Id
    }
```

The record gives the flow a named boundary, and `Flow.read` keeps each access local to the piece
the flow actually needs.

Records start to get noisy when:

- every helper takes a bigger and bigger parameter bag
- you keep threading the same deps through unrelated steps
- the dependency surface matters more than the actual data
- the code wants to say “this effect needs logging, clock, and db” instead of “it needs a blob”

At that point a named cap gives the compiler a dependency surface to check and gives the reader a
sentence instead of a bag of fields.

## Support For Records

FsFlow supports records directly through the same `Flow` helpers:

- `Flow.env` to get the whole record
- `Flow.read` to project a single field or computed value
- `Flow.localEnv` to reshape the record for a smaller scope
- `Flow.provideLayer` when a downstream flow should run against a derived environment

That means records stay a direct, supported shape for dependency handling in `env`.

## When Caps Help

A capability contract puts a name on the dependency surface and keeps it in the type:

- it makes the effect’s requirements visible in the type
- it gives refactoring a boundary the compiler can check
- it behaves like programming to interfaces, but for effects
- it keeps flow code honest about what it depends on

That makes caps attractive even when a record would still work.

## More Direct Dependency Management

Two mechanisms stay deliberately separate:

- `env` carries the application dependency shape that workflow code reads with `Flow.env` and
  `Flow.read`.
- runtime overrides carry operational services such as clock, logging, random, GUID generation,
  and environment-variable lookup.

The internal registry, scope, adapter, and runtime layer machinery support hosting and generated
reference behavior. They are not the public dependency model for ordinary workflows. User code
should normally choose between a record, a nominal capability contract, and a host-edge adapter.

## Choosing A Shape

The record shape gives you a concrete boundary and direct field access.

```fsharp
type ApiDeps =
    { Orders : IOrderRepository
      Email : IEmailSender
      Clock : IClock }
```

Runtime overrides keep clock, logging, and similar services out of the visible environment. They
are implicit in normal flows, and test code can override them locally.

Standard `.NET` AppHost plus DI adapts the container into a boundary record or nominal contract
once, then workflow code stays on typed values.

Nominal capability contracts let a helper or workflow name the capabilities it needs and give the
compiler a surface to check.

## Architectural Styles

All of these styles are still about `env`.

The question is not whether FsFlow uses `env`. The question is which shape makes the dependency
boundary easiest to read, test, and evolve.

### 1. Booted App Environment

This style keeps `env` as one concrete record that holds the dependencies for the whole app.

```fsharp
type AppEnv =
    { Db : IDb
      Log : string -> unit
      Config : AppConfig
      Billing : IBillingClient
      Cache : ICache }

let handle command : Flow<AppEnv, AppError, ResultValue> =
    flow {
        let! env = Flow.env
        env.Log "handling command"
        return! runWorkflow env.Db env.Billing command
    }
```

This style gives you:

- controllers and endpoints
- handlers
- persistence workflows
- infrastructure-heavy orchestration
- integration tests

The benefit is directness: the workflow sees one concrete application environment and the host
construction stays obvious.

### 2. Explicit Dependencies Plus Context

This style keeps the request or execution context small and passes the real dependencies as named
inputs.

```fsharp
type PlaceOrderDeps =
    { LoadCart : CartId -> Flow<RequestContext, AppError, Cart>
      SaveOrder : Order -> Flow<RequestContext, AppError, unit>
      PublishEvent : OrderPlaced -> Flow<RequestContext, AppError, unit> }

type RequestContext =
    { TraceId : string
      UserId : string
      Deadline : System.DateTimeOffset }

let placeOrder
    (deps : PlaceOrderDeps)
    (input : PlaceOrderInput)
    : Flow<RequestContext, AppError, OrderId> =
    flow {
        let! ctx = Flow.env
        let! cart = deps.LoadCart input.CartId
        let order = Order.create ctx.UserId cart
        do! deps.SaveOrder order
        do! deps.PublishEvent (OrderPlaced order.Id)
        return order.Id
    }
```

This style gives you:

- use cases
- domain orchestration
- workflow modules
- highly testable feature logic

The benefit is locality: each helper says what it needs, and the request or execution context stays
small and focused.

This style maps well to nominal capability contracts because the type itself carries the dependency
surface. It maps to a concrete record when the dependency list is local and obvious.

### 3. Runtime-Owned Services

This style keeps operational services out of the visible environment and supplies them at the
execution boundary.

```fsharp
let handle command =
    flow {
        let! env = Flow.env
        do! Log.info $"handling {command.Id}"
        let! now = Clock.now
        return! runWorkflow env command now
    }
    |> Flow.withClock Clock.live
    |> Flow.withLog Log.live
```

This style gives you:

- runtime-owned clock, logging, random, and guid services
- a clean app environment that stays focused on business dependencies
- local overrides for tests and scoped changes

The benefit is separation: the app sees only its own dependencies, while the runtime owns the
operational services and tests can swap them without changing the app environment.

### 4. Standard `.NET` AppHost Plus DI

This style keeps the host conventional and turns registrations into a boundary record or nominal
contract once.

```fsharp
type RuntimeServices =
    { Logger : ILogger<ShipOrderWorkflow> }

type AppEnv =
    { Gateway : IShippingGateway }

type ShipOrderWorkflow() =
    member _.Run(input : ShipOrderInput) : Flow<AppEnv, AppError, ShipmentId> =
        flow {
            let! gateway = Flow.read _.Gateway

            do! Log.info $"shipping order {input.OrderId}"
            let! shipmentId = gateway.CreateShipment(input.OrderId)
            return shipmentId
        }
```

This style gives you:

- mixed C# and F# teams
- enterprise `.NET` apps
- incremental adoption

The benefit is that the host remains familiar while workflow code can still use explicit contracts
and runtime overrides where they fit.

This style maps directly onto standard `.NET` AppHost Plus DI because the host container can be
adapted into typed values once. It maps into nominal capability contracts when the workflow code
should speak in named capabilities rather than container lookups.

## How The Pieces Fit Together

The dependency story is:

- host registrations are adapted into a boundary record or nominal contract at the edge
- public workflows consume `env` or app capability contracts, not lookup machinery
- runtime overrides carry operational services that should not widen `env`
- `Resolver.fromProvider` is an edge binding helper, not the main application shape
- `Layer.provideLayer` derives one `env` from another; it is not a service container

That is the shape the rest of the dependency docs follow.
