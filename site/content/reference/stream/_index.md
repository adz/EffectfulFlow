---
title: "Stream"
type: docs
---

The `FlowStream` module provides asynchronous, pull-based streams.

## Core type

- [`FlowStream`](./t-flowstream-3.md): 
 Represents a cold stream of values that requires an environment, can fail with a typed error,
 and supports backpressure.
 

## Module functions

- [`FlowStream.fromSeq`](./m-flowstream-fromseq.md): Creates a stream from a sequence of values.
- [`FlowStream.map`](./m-flowstream-map.md): Maps the successful values of a stream.
- [`FlowStream.runForEach`](./m-flowstream-runforeach.md): Executes the stream and performs an action for each value.

