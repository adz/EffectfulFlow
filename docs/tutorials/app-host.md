---
weight: 40
title: "Tutorial: AppHost"
description: Integration with .NET Generic Host and Dependency Injection.
---

# Tutorial: AppHost

In enterprise .NET applications, you typically use a dependency injection (DI) container managed by `IHostBuilder` or `IWebHostBuilder`. FsFlow keeps that at the boundary: adapt the container into a plain record once, then use `Flow` with runtime overrides for clocking and logging. Those runtime services are implicit in normal flows and only become visible when you override them for tests or a local scope.

This tutorial shows a practical boundary:

- `IServiceProvider` is only used in the host layer
- your app dependencies stay in a typed record
- runtime services like clock and log are implicit in normal flows, and `Flow.withClock` / `Flow.withLog` let you override them when you need test control

## 1. Register Services in the Container

Register your application services as you normally would.

```fsharp
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

let host =
    Host.CreateDefaultBuilder()
        .ConfigureServices(fun services ->
            services.AddSingleton<IOrderRepository, InMemoryOrders>()
            services.AddSingleton<IEmailSender, ConsoleEmail>()
        )
        .Build()
```

## 2. Adapt The Provider Once

Turn the provider into a typed boundary record at the edge.

```fsharp
type AppEnv =
    { Orders: IOrderRepository
      Email: IEmailSender }

let createAppEnv (sp: IServiceProvider) =
    { Orders = sp.GetRequiredService<IOrderRepository>()
      Email = sp.GetRequiredService<IEmailSender>() }
```

## 3. Write The Workflow

Use the record directly and keep runtime concerns separate.

```fsharp
open FsFlow
open FsFlow.Capabilities.Core

let placeOrder order =
    flow {
        let! env = Flow.env
        do! Log.info (sprintf "Placing order %A" order.Id)
        do! env.Orders.Save order
        do! env.Email.SendConfirmation order
        return order.Id
    }
```

## 4. Run The Workflow

Inject clock and logging at the boundary when you want deterministic behavior, then run the workflow against the app record.

```fsharp
let run order =
    let appEnv = createAppEnv host.Services

    placeOrder order
    |> Flow.withClock Clock.live
    |> Flow.withLog Log.live
    |> Flow.run appEnv
```

## Why Use AppHost Integration?

- **Interoperability**: Works with existing ASP.NET Core and worker-service setups.
- **Boundary Clarity**: The app record is typed; runtime services stay runtime-owned and test-overridable.
- **Gradual Migration**: You can adapt a provider once and keep the rest of the workflow on simple values.

## Summary

The default FsFlow pattern is now:

1. use `AppRecord` or `Capabilities` for app dependencies
2. use `Flow.withClock`, `Flow.withLog`, `Flow.withRandom`, and `Flow.withGuid` only when you need to override the implicit runtime services
3. use `IServiceProvider` only at the host edge
