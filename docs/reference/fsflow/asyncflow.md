---
title: AsyncFlow
description: Source-documented async workflow surface in FsFlow.
---

# AsyncFlow

This page shows the source-documented `AsyncFlow` surface: the core type, the module functions, and the `asyncFlow { }` builder.

## Core type

- type `AsyncFlow`: Represents a cold async workflow that reads an environment, returns a typed result,
and is executed explicitly through `AsyncFlow.run`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Core.fs#L24)

## Builder

- [`Builders.asyncFlow`](./builders-asyncflow.md): The core `asyncFlow { }` computation expression. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Builders.fs#L442)

## Module functions

- module `AsyncFlow` [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L6)
- [`AsyncFlow.run`](./asyncflow-run.md): Executes an async flow with the provided environment. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L8)
- [`AsyncFlow.toAsync`](./asyncflow-toasync.md): Converts an async flow into its raw async result shape. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L15)
- [`AsyncFlow.succeed`](./asyncflow-succeed.md): Creates a successful async flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L19)
- [`AsyncFlow.fail`](./asyncflow-fail.md): Creates a failing async flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L23)
- [`AsyncFlow.fromResult`](./asyncflow-fromresult.md): Lifts a `Result` into an async flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L27)
- [`AsyncFlow.fromOption`](./asyncflow-fromoption.md): Lifts an option into an async flow with the supplied error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L31)
- [`AsyncFlow.fromValueOption`](./asyncflow-fromvalueoption.md): Lifts a value option into an async flow with the supplied error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L37)
- [`AsyncFlow.orElseAsync`](./asyncflow-orelseasync.md): Turns a pure validation result into an async flow with async-provided failure. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L44)
- [`AsyncFlow.orElseAsyncFlow`](./asyncflow-orelseasyncflow.md): Turns a pure validation result into an async flow whose failure value comes from another async flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L58)
- [`AsyncFlow.fromFlow`](./asyncflow-fromflow.md): Lifts a synchronous flow into an async flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L75)
- [`AsyncFlow.fromAsync`](./asyncflow-fromasync.md): Lifts an async value into an async flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L79)
- [`AsyncFlow.fromAsyncResult`](./asyncflow-fromasyncresult.md): Lifts an async result into an async flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L87)
- [`AsyncFlow.env`](./asyncflow-env.md): Reads the current environment as the flow value. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L91)
- [`AsyncFlow.read`](./asyncflow-read.md): Projects a value from the current environment. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L95)
- [`AsyncFlow.map`](./asyncflow-map.md): Maps the successful value of an async flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L99)
- [`AsyncFlow.bind`](./asyncflow-bind.md): Sequences an async continuation after a successful value. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L115)
- [`AsyncFlow.tap`](./asyncflow-tap.md): Runs an async side effect on success and preserves the original value. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L135)
- [`AsyncFlow.tapError`](./asyncflow-taperror.md): Runs an async side effect on failure and preserves the original error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L146)
- [`AsyncFlow.mapError`](./asyncflow-maperror.md): Maps the error value of an async flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L165)
- [`AsyncFlow.catch`](./asyncflow-catch.md): Catches exceptions raised during execution and maps them to a typed error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L181)
- [`AsyncFlow.orElse`](./asyncflow-orelse.md): Falls back to another async flow when the source flow fails. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L194)
- [`AsyncFlow.zip`](./asyncflow-zip.md): Combines two async flows into a tuple of their values. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L208)
- [`AsyncFlow.map2`](./asyncflow-map2.md): Combines two async flows with a mapping function. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L219)
- [`AsyncFlow.localEnv`](./asyncflow-localenv.md): Transforms the environment before running the async flow. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L228)
- [`AsyncFlow.delay`](./asyncflow-delay.md): Defers async flow construction until execution time. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L235)
- [`AsyncFlow.traverse`](./asyncflow-traverse.md): Transforms a sequence of values into an async flow and stops at the first failure. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L239)
- [`AsyncFlow.sequence`](./asyncflow-sequence.md): Transforms a sequence of async flows into an async flow of a sequence and stops at the first failure. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/AsyncFlow.fs#L262)

