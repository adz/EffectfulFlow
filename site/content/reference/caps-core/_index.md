---
title: "CAPS Core"
weight: 131
type: docs
---

`FsFlow.Caps.Core` is the smallest shared capability package in the FsFlow CAPS story. It keeps the surface synchronous and explicit: clock, random, GUID, and environment-variable capabilities.

## Capability types

- [`Core.IClock`](./t-core-iclock.md): Provides synchronous access to the current UTC clock.
- [`Core.IRandom`](./t-core-irandom.md): Provides synchronous random-number generation.
- [`Core.IGuid`](./t-core-iguid.md): Provides synchronous GUID generation.
- [`Core.IEnvironmentVariables`](./t-core-ienvironmentvariables.md): Provides synchronous environment-variable lookup.
- [`Core.EnvironmentVariableError`](./t-core-environmentvariableerror.md): Describes a meaningful environment-variable failure.

## Clock

- [`Core.Clock.now`](./m-core-clock-now.md): Reads the current UTC timestamp from the environment.
- [`Core.Clock.live`](./m-core-clock-live.md): Creates a live clock backed by <a href="https://learn.microsoft.com/dotnet/api/system.datetimeoffset.utcnow">DateTimeOffset.UtcNow</a>.
- [`Core.Clock.fromValue`](./m-core-clock-fromvalue.md): Creates a deterministic clock that always returns the supplied instant.

## Random

- [`Core.Random.nextInt`](./m-core-random-nextint.md): Reads a random integer from the environment.
- [`Core.Random.live`](./m-core-random-live.md): Creates a live random-number generator backed by <a href="https://learn.microsoft.com/dotnet/api/system.random">Random</a>.
- [`Core.Random.fromValue`](./m-core-random-fromvalue.md): Creates a deterministic random generator that always returns the supplied value.

## GUID

- [`Core.Guid.newGuid`](./m-core-guid-newguid.md): Reads a GUID from the environment.
- [`Core.Guid.live`](./m-core-guid-live.md): Creates a live GUID generator backed by <a href="https://learn.microsoft.com/dotnet/api/system.guid.newguid">Guid.NewGuid</a>.
- [`Core.Guid.fromValue`](./m-core-guid-fromvalue.md): Creates a deterministic GUID generator that always returns the supplied value.

## Environment variables

- [`Core.EnvironmentVariables.tryGet`](./m-core-environmentvariables-tryget.md): Reads a raw environment-variable value from the environment.
- [`Core.EnvironmentVariables.live`](./m-core-environmentvariables-live.md): Creates a live provider backed by the current process environment.
- [`Core.EnvironmentVariables.fromPairs`](./m-core-environmentvariables-frompairs.md): Creates a deterministic provider from a fixed set of name/value pairs.
- [`Core.EnvironmentVariable.tryGet`](./m-core-environmentvariable-tryget.md): Reads a raw string environment variable without wrapping it in a result.
- [`Core.EnvironmentVariable.get`](./m-core-environmentvariable-get.md): Reads a raw string environment variable from the environment.
- [`Core.EnvironmentVariable.getInt`](./m-core-environmentvariable-getint.md): Reads an integer environment variable from the environment.
- [`Core.EnvironmentVariable.getGuid`](./m-core-environmentvariable-getguid.md): Reads a GUID environment variable from the environment.
- [`Core.EnvironmentVariable.getBool`](./m-core-environmentvariable-getbool.md): Reads a boolean environment variable from the environment.
- [`Core.EnvironmentVariableErrors.describe`](./m-core-environmentvariableerrors-describe.md): Formats a human-readable description for an error.

