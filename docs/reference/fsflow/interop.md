---
title: Interop
description: Source-documented task and async interop helpers for FsFlow.
---

# Interop

This page shows the interop helpers that bridge task, async, and synchronous boundaries in FsFlow.

## TaskFlow bridges

- [`TaskFlow.fromFlow`](./taskflow-fromflow.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/TaskFlow.fs#L213)
- [`TaskFlow.fromAsyncFlow`](./taskflow-fromasyncflow.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/TaskFlow.fs#L216)
- [`TaskFlow.orElseTask`](./taskflow-orelsetask.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/TaskFlow.fs#L137)
- [`TaskFlow.orElseAsync`](./taskflow-orelseasync.md): Turns a pure validation result into a task flow with task-provided failure. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/TaskFlow.fs#L152)
- [`TaskFlow.orElseFlow`](./taskflow-orelseflow.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/TaskFlow.fs#L165)
- [`TaskFlow.orElseAsyncFlow`](./taskflow-orelseasyncflow.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/TaskFlow.fs#L179)
- [`TaskFlow.orElseTaskFlow`](./taskflow-orelsetaskflow.md) [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/TaskFlow.fs#L197)

## Builder extensions

- module `TaskFlowBuilderExtensions` [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/TaskFlow.fs#L988)
- module `AsyncFlowBuilderExtensions`: [omit] [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/TaskFlow.fs#L1062)

