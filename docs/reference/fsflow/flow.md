---
title: Flow
description: Source-documented synchronous workflow surface in FsFlow.
---

# Flow

This page shows the source-documented `Flow` surface: the core type, the module functions, and the `flow { }` builder.

## Core type

- type `Flow`: Represents a cold synchronous workflow that reads an environment, returns a typed result,
and is executed explicitly through `Flow.run`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Core.fs#L13)

## Builder

- [`Builders.flow`](./builders-flow.md): The sync-only `flow { }` computation expression. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Builders.fs#L412)

## Module functions

- module `Flow` [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L6)
- [`Flow.run`](./flow-run.md): Executes a synchronous flow with the provided environment. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L15)
- [`Flow.succeed`](./flow-succeed.md): Creates a successful synchronous flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L26)
- [`Flow.value`](./flow-value.md): Alias for `succeed` that reads well in some call sites. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L35)
- [`Flow.fail`](./flow-fail.md): Creates a failing synchronous flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L46)
- [`Flow.fromResult`](./flow-fromresult.md): Lifts a `Result` into a synchronous flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L56)
- [`Flow.fromOption`](./flow-fromoption.md): Lifts an option into a synchronous flow with the supplied error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L66)
- [`Flow.fromValueOption`](./flow-fromvalueoption.md): Lifts a value option into a synchronous flow with the supplied error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L75)
- [`Flow.orElseFlow`](./flow-orelseflow.md): Turns a pure validation result into a synchronous flow with environment-provided failure. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L89)
- [`Flow.env`](./flow-env.md): Reads the current environment as the flow value. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L107)
- [`Flow.read`](./flow-read.md): Projects a value from the current environment. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L117)
- [`Flow.map`](./flow-map.md): Maps the successful value of a synchronous flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L128)
- [`Flow.bind`](./flow-bind.md): Sequences a synchronous continuation after a successful value. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L142)
- [`Flow.tap`](./flow-tap.md): Runs a synchronous side effect on success and preserves the original value. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L166)
- [`Flow.tapError`](./flow-taperror.md): Runs a synchronous side effect on failure and preserves the original error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L185)
- [`Flow.mapError`](./flow-maperror.md): Maps the error value of a synchronous flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L205)
- [`Flow.catch`](./flow-catch.md): Catches exceptions raised during execution and maps them to a typed error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L224)
- [`Flow.orElse`](./flow-orelse.md): Falls back to another flow when the source flow fails. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L235)
- [`Flow.zip`](./flow-zip.md): Combines two flows into a tuple of their values. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L245)
- [`Flow.map2`](./flow-map2.md): Combines two flows with a mapping function. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L256)
- [`Flow.localEnv`](./flow-localenv.md): Transforms the environment before running the flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L265)
- [`Flow.delay`](./flow-delay.md): Defers flow construction until execution time. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L272)
- [`Flow.traverse`](./flow-traverse.md): Transforms a sequence of values into a flow and stops at the first failure. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L276)
- [`Flow.sequence`](./flow-sequence.md): Transforms a sequence of flows into a flow of a sequence and stops at the first failure. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L295)

