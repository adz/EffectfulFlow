---
title: "Flow"
weight: 10
type: docs
---

This page shows the source-documented `Flow` surface: the core type and module functions.

## Core type

- [`Flow`](./t-flow-3.md): 
 Represents a cold workflow that reads an environment, returns a typed result, and is executed
 explicitly through <code>Flow.run</code>.
 

## Module functions

- [`Flow.run`](./m-flow-run.md): Executes a flow with the provided environment and the default cancellation token.
- [`Flow.ok`](./m-flow-ok.md): Creates a successful synchronous flow.
- [`Flow.error`](./m-flow-error.md): Creates a failing synchronous flow.
- [`Flow.succeed`](./m-flow-succeed.md): Alias for <a href="https://learn.microsoft.com/dotnet/api/ok">ok</a> that reads well in some call sites.
- [`Flow.value`](./m-flow-value.md): Alias for <a href="https://learn.microsoft.com/dotnet/api/ok">ok</a> that reads well in some call sites.
- [`Flow.fail`](./m-flow-fail.md): Alias for <a href="https://learn.microsoft.com/dotnet/api/error">error</a> that reads well in some call sites.
- [`Flow.fromResult`](./m-flow-fromresult.md): Lifts a <a href="https://learn.microsoft.com/dotnet/api/system.result-2">Result</a> into a synchronous flow.
- [`Flow.fromOption`](./m-flow-fromoption.md): Lifts an option into a synchronous flow with the supplied error.
- [`Flow.fromValueOption`](./m-flow-fromvalueoption.md): Lifts a value option into a synchronous flow with the supplied error.
- [`Flow.orElseFlow`](./m-flow-orelseflow.md): Turns a pure validation result into a synchronous flow with environment-provided failure.
- [`Flow.env`](./m-flow-env.md): Reads the current environment as the flow value.
- [`Flow.read`](./m-flow-read.md): Projects a value from the current environment.
- [`Flow.map`](./m-flow-map.md): Maps the successful value of a synchronous flow.
- [`Flow.bind`](./m-flow-bind.md): Sequences a synchronous continuation after a successful value.
- [`Flow.tap`](./m-flow-tap.md): Runs a synchronous side effect on success and preserves the original value.
- [`Flow.tapError`](./m-flow-taperror.md): Runs a synchronous side effect on failure and preserves the original error.
- [`Flow.mapError`](./m-flow-maperror.md): Maps the error value of a synchronous flow.
- [`Flow.catch`](./m-flow-catch.md): Catches exceptions raised during execution and maps them to a typed error.
- [`Flow.orElseWith`](./m-flow-orelsewith.md): Falls back to another flow when the source flow fails.Computes a fallback flow from the source error when the source flow fails.
- [`Flow.orElse`](./m-flow-orelse.md): Falls back to another flow when the source flow fails.
- [`Flow.zip`](./m-flow-zip.md): Combines two flows into a tuple of their values.
- [`Flow.map2`](./m-flow-map2.md): Combines two flows with a mapping function.
- [`Flow.map3`](./m-flow-map3.md): Combines three flows with a mapping function.
- [`Flow.apply`](./m-flow-apply.md): Applies a flow-wrapped function to a flow-wrapped value.
- [`Flow.ignore`](./m-flow-ignore.md): Maps the successful value of a synchronous flow to <code>unit</code>.
- [`Flow.localEnv`](./m-flow-localenv.md): Transforms the environment before running the flow.
- [`Flow.provideLayer`](./m-flow-providelayer.md): Provides a derived environment from a layer flow to a downstream flow.
- [`Flow.delay`](./m-flow-delay.md): Defers flow construction until execution time.
- [`Flow.traverse`](./m-flow-traverse.md): Transforms a sequence of values into a flow and stops at the first failure.
- [`Flow.sequence`](./m-flow-sequence.md): Transforms a sequence of flows into a flow of a sequence and stops at the first failure.

## Concurrency

- [`Fiber`](./t-fiber-2.md): 
 Represents a handle to a running workflow.
 
- [`Flow.fork`](./m-flow-fork.md): Starts a flow in a new fiber without waiting for it to complete.
- [`Flow.join`](./m-flow-join.md): Waits for a fiber to complete and returns its final outcome.
- [`Flow.interrupt`](./m-flow-interrupt.md): Signals a fiber to stop and waits for it to finish its cleanup.

## Parallel orchestration

- [`Flow.zipPar`](./m-flow-zippar.md): Combines two flows into a tuple of their values, running them concurrently.
- [`Flow.race`](./m-flow-race.md): Runs two flows concurrently and returns the result of the first one to complete.

