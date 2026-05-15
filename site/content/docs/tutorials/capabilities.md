---
weight: 30
title: "Tutorial: Capabilities"
description: Using nominal interface contracts as the environment.
type: docs
---


Nominal Capability Contracts use F# interfaces to name app dependencies. This lets the compiler
check that your environment implements the required capabilities and makes it easy to reuse
helpers across different workflows.

In this tutorial, we will refactor our workflow to use interface-based app capabilities instead of
concrete records.

## 1. Define Capability Interfaces

Instead of one big record, define small, focused interfaces.

```fsharp
open System
open System.Threading.Tasks
open FsFlow

type IHasOrders =
    abstract Orders: IOrderRepository

type IHasEmail =
    abstract Email: IEmailSender

// You can also group them into a larger contract
type IAppCaps =
    inherit IHasOrders
    inherit IHasEmail
```

Runtime-owned services like clock and logging are not part of this contract. They are implicit in
the runtime, and you override them with `Flow.withClock`, `Flow.withLog`, `Flow.withRandom`, or
`Flow.withGuid` when you need test control or a local scope change.

## 2. Write Helpers using Capability Constraints

Helper functions can now specify exactly which app capabilities they need using the `#` flexible
type constraint.

```fsharp
let saveOrder order : Flow<#IHasOrders, _, _> =
    flow {
        let! repo = Flow.read _.Orders
        do! repo.Save order
    }

let sendEmail order : Flow<#IHasEmail, _, _> =
    flow {
        let! sender = Flow.read _.Email
        do! sender.SendConfirmation order
    }
```

## 3. Compose the Main Workflow

The main workflow combines these helpers. The compiler will infer that the environment must
implement both `IHasOrders` and `IHasEmail`. Runtime services are still implicit, so nothing extra
is threaded through `env` for clock or logging here.

```fsharp
let placeOrder order =
    flow {
        do! saveOrder order
        do! sendEmail order
        return order.Id
    }
```

## 4. Implement the Environment

Your application environment is now just a class or record that implements the required app
interfaces.

```fsharp
type AppEnv =
    { Orders: IOrderRepository
      Email: IEmailSender }
    interface IHasOrders with member x.Orders = x.Orders
    interface IHasEmail with member x.Email = x.Email

[<EntryPoint>]
let main _ =
    let env =
        { Orders = InMemoryOrders()
          Email = ConsoleEmail() }
    let order = { Id = Guid.NewGuid(); Total = 99.99m }

    // env matches both #IHasOrders and #IHasEmail
    let run () = task {
        let! result = Flow.run env (placeOrder order)
        printfn "Result: %A" result
    }

    run().GetAwaiter().GetResult()
```

## 5. Override Runtime Services In Tests

When you need deterministic clocking or logging, override the implicit runtime services locally
without changing the app contract.

```fsharp
open FsFlow.Capabilities.Core

let testRun () =
    let env =
        { Orders = InMemoryOrders()
          Email = ConsoleEmail() }

    let fixedClock = Clock.fromValue (DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero))
    let order = { Id = Guid.NewGuid(); Total = 42m }

    flow {
        do! Log.info "running test"
        let! timestamp = Clock.now
        let! result = placeOrder order
        return timestamp, result
    }
    |> Flow.withClock fixedClock
    |> Flow.withLog Log.live
    |> Flow.run env
```

## Why use Capabilities?

- **Refactor Safety**: If you add a new app capability to a helper, the compiler will immediately tell you every call site that needs to be updated.
- **Granular Dependencies**: Helpers only ask for what they actually need, making the code easier to reason about and test.
- **Reusable Logic**: You can write general-purpose helpers that work on any environment that provides the required app capabilities.
- **Testable Runtime**: Clock and logging stay implicit by default, but you can override them locally when a test needs a fixed value.

## Next Steps

For enterprise applications that use standard .NET dependency injection, proceed to the **[AppHost](./app-host/)** tutorial to see how to bridge `IServiceProvider` into the FsFlow world.
