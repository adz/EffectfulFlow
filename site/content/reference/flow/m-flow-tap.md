---
title: "Flow.tap"
linkTitle: "tap"
type: docs
---

<div class="fsdocs-usage">
<code><span>tap&#32;<span>binder&#32;flow</span></span></code>
</div>

Runs a synchronous side effect on success and preserves the original value.

## Remarks


 Use this for logging, telemetry, or other &quot;fire and forget&quot; operations that should not
 alter the primary value path. If the <span class="fsdocs-param-name">binder</span> flow fails, the entire
 flow fails with that error.
 

## Parameters

- `binder`: <code><span>'value&#32;->&#32;<span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;unit</span>&gt;</span></span></code>
  A function that produces a side-effect flow from the successful value.
- `flow`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></code>
  The source flow.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> that preserves the original success value after the side effect.

