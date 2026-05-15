---
title: "Capability"
---

In FsFlow, a capability is a named interface that describes what a flow needs from `env`. A capability contract puts that interface in the environment surface type. Using an interface through a capability contract makes the dependency visible in the type, so the compiler can check it, refactoring can move safely, and reusable helpers can advertise what they need. For ordinary workflow code, prefer small app contracts plus `Flow.read`. Runtime-owned services such as clock, logging, random, GUID generation, and environment variables are read through `FsFlow.Capabilities.Core` helpers and overridden with `Flow.withClock`, `Flow.withLog`, `Flow.withRandom`, `Flow.withGuid`, and `Flow.withEnvironmentVariables`. This page shows the source-documented compatibility binding tokens, host-edge helpers, and layer helper.

## Binding tokens

- [`Requires`](./t-requires-1.md): Compatibility contract for a single dependency.
- [`Resolve`](./t-resolve-1.md): Request token for binding a whole dependency inside a workflow.
- [`Resolve`](./t-resolve-2.md): Request token for projecting a value from a dependency.

## Edge helpers

- [`MissingCapability`](./t-missingcapability.md): Describes a missing service-provider capability.
- [`Resolver.resolve`](./m-resolver-resolve.md): Reads a dependency from the environment using the provided projection.
- [`Resolver.fromProvider`](./m-resolver-fromprovider.md): Reads a dependency from <a href="https://learn.microsoft.com/dotnet/api/iserviceprovider">IServiceProvider</a> and fails when it is not registered.

## Layers

- [`Layer.provideLayer`](./m-layer-providelayer.md): Provides a derived environment from a layer flow to a downstream flow.

