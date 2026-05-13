---
title: "Capability"
weight: 130
type: docs
---

This page shows the source-documented capability and layer surface, including CAPS request tokens and environment management helpers.

## CAPS tokens

- [`Needs`](./t-needs-1.md): Describes the capability contract for a single dependency.
- [`Env`](./t-env-1.md): Request token for binding a whole dependency inside a workflow.
- [`Env`](./t-env-2.md): Request token for projecting a value from a dependency.

## Capabilities

- [`MissingCapability`](./t-missingcapability.md): Describes a missing service-provider capability.
- [`Capability.service`](./m-capability-service.md): Reads a service from the environment using the provided projection.
- [`Capability.runtime`](./m-capability-runtime.md): Reads the current runtime from the environment.
- [`Capability.environment`](./m-capability-environment.md): Reads the application environment from the environment.
- [`Capability.serviceFromProvider`](./m-capability-servicefromprovider.md): Reads a service from <a href="https://learn.microsoft.com/dotnet/api/iserviceprovider">IServiceProvider</a> and fails when it is not registered.

## Layers

- [`Layer.provideLayer`](./m-layer-providelayer.md): Provides a derived environment from a layer flow to a downstream flow.

