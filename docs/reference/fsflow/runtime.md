---
title: AsyncFlow.Runtime
description: Source-documented async runtime support and helpers for FsFlow.
---

# AsyncFlow.Runtime

This page shows the source-documented `AsyncFlow.Runtime` surface: logging, retry policies, and async operational helpers.

## Logging

- type `LogLevel`: Log levels used by runtime logging helpers and environment-provided logging functions. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Core.fs#L32)
- type `LogEntry`: A structured log entry written through a runtime logger. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Core.fs#L43)

## Retry policy

- type `RetryPolicy`: Defines how runtime retry helpers repeat typed failures in a controlled way. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Core.fs#L53)
- module `RetryPolicy`: Standard retry policies for runtime helpers. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Core.fs#L64)
- [`RetryPolicy.noDelay`](./retrypolicy-nodelay.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Core.fs#L65)

## Async operational helpers

- module `AsyncFlow.Runtime`: Runtime helpers for operational concerns like logging, timeout, retry, and cleanup. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L269)
- [`AsyncFlow.Runtime.cancellationToken`](./asyncflow-runtime-cancellationtoken.md): Reads the current cancellation token from the flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L277)
- [`AsyncFlow.Runtime.catchCancellation`](./asyncflow-runtime-catchcancellation.md): Catches `OperationCanceledException` and converts it into a typed error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L290)
- [`AsyncFlow.Runtime.ensureNotCanceled`](./asyncflow-runtime-ensurenotcanceled.md): Checks if cancellation has been requested and returns a typed error if it has. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L307)
- [`AsyncFlow.Runtime.sleep`](./asyncflow-runtime-sleep.md): Suspends the flow for the specified duration, observing cancellation. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L323)
- [`AsyncFlow.Runtime.log`](./asyncflow-runtime-log.md): Writes a log entry using the writer provided by the environment. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L337)
- [`AsyncFlow.Runtime.logWith`](./asyncflow-runtime-logwith.md): Writes a log entry using a message produced from the environment. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L359)
- [`AsyncFlow.Runtime.useWithAcquireRelease`](./asyncflow-runtime-usewithacquirerelease.md): Safely acquires a resource, uses it, and ensures it is released via a task-based action. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L381)
- [`AsyncFlow.Runtime.timeout`](./asyncflow-runtime-timeout.md): Wraps a flow with a timeout. If the flow does not complete within the specified duration, returns a typed error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L409)
- [`AsyncFlow.Runtime.timeoutToOk`](./asyncflow-runtime-timeouttook.md): Wraps a flow with a timeout. If the flow does not complete within the specified duration, returns a success value. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L433)
- [`AsyncFlow.Runtime.timeoutToError`](./asyncflow-runtime-timeouttoerror.md): Transitions to a failure value on timeout. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L457)
- [`AsyncFlow.Runtime.timeoutWith`](./asyncflow-runtime-timeoutwith.md): Transitions to a fallback workflow on timeout. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L467)
- [`AsyncFlow.Runtime.retry`](./asyncflow-runtime-retry.md): Retries a flow according to the specified policy. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L493)

