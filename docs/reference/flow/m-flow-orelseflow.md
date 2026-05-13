---
title: "Flow.orElseFlow"
linkTitle: "orElseFlow"
---

<div class="fsdocs-usage">
<code><span>orElseFlow&#32;<span>errorFlow&#32;result</span></span></code>
</div>

Turns a pure validation result into a synchronous flow with environment-provided failure.

## Remarks


 This helper bridges the gap between pure validation (which often uses <a href="https://learn.microsoft.com/dotnet/api/system.result-2">Result</a> or <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>)
 and the <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> environment model. If the result is an error, the provided <span class="fsdocs-param-name">errorFlow</span>
 is executed to produce the final application error.
 

## Parameters

- `errorFlow`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'error</span>&gt;</span></code>
  A flow that reads the environment to produce an error value.
- `result`: <code><span><a href="https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-fsharpresult-2">Result</a>&lt;<span>'value,&#32;unit</span>&gt;</span></code>
  The pure result to bridge.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> that mirrors the success of the result or fails with the outcome of the error flow.

