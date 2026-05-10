---
title: Interop
---

This page shows the interop helpers that bridge task, async, and synchronous boundaries in FsFlow.

## TaskFlow bridges

- [`TaskFlow.fromFlow`](./taskflow-fromflow.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1253)
- [`TaskFlow.fromAsyncFlow`](./taskflow-fromasyncflow.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1256)
- [`TaskFlow.orElseTask`](./taskflow-orelsetask.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1177)
- [`TaskFlow.orElseAsync`](./taskflow-orelseasync.md): Turns a pure validation result into a task flow with task-provided failure. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1192)
- [`TaskFlow.orElseFlow`](./taskflow-orelseflow.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1205)
- [`TaskFlow.orElseAsyncFlow`](./taskflow-orelseasyncflow.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1219)
- [`TaskFlow.orElseTaskFlow`](./taskflow-orelsetaskflow.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1237)
- [`Flow.provideLayer`](./flow-providelayer.md): Provides a derived environment from a layer flow to a downstream flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L345)
- [`AsyncFlow.provideLayer`](./asyncflow-providelayer.md): Provides a derived environment from a layer flow to a downstream flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L691)
- [`TaskFlow.provideLayer`](./taskflow-providelayer.md): Provides a derived environment from a layer flow to a downstream task flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1561)

## Builder extensions

- module `TaskFlowBuilderExtensions` [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L2282)
- module `AsyncFlowBuilderExtensions` [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Guard.fs#L190)

