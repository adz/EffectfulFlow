---
title: "Flow.mapError"
linkTitle: "mapError"
---

<div class="fsdocs-usage">
<code><span>mapError&#32;<span>mapper&#32;flow</span></span></code>
</div>

Maps the error value of a synchronous flow.

## Remarks


 Transforms the error type of the flow while leaving successful values untouched.
 Useful for mapping internal errors into public-facing domain errors.
 

## Parameters

- `mapper`: <code><span>'error&#32;->&#32;'nextError</span></code>
  The function to transform the error value.
- `flow`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></code>
  The source flow.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> with the transformed error type.

