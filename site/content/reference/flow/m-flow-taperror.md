---
title: "Flow.tapError"
linkTitle: "tapError"
type: docs
---

<div class="fsdocs-usage">
<code><span>tapError&#32;<span>binder&#32;flow</span></span></code>
</div>

Runs a synchronous side effect on failure and preserves the original error.

## Remarks


 Use this for error logging or cleanup actions that depend on the environment.
 If the <span class="fsdocs-param-name">binder</span> side-effect flow itself fails, its error will
 overwrite the original error.
 

## Parameters

- `binder`: <code><span>'error&#32;->&#32;<span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;unit</span>&gt;</span></span></code>
  A function that produces a side-effect flow from the error value.
- `flow`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></code>
  The source flow.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> that preserves the original error after the side effect.

