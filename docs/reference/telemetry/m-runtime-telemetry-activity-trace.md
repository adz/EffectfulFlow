---
title: "Runtime.Telemetry.Activity.trace"
linkTitle: "trace"
---

<div class="fsdocs-usage">
<code><span>Activity.trace&#32;<span>name&#32;flow</span></span></code>
</div>


 Wraps a flow in a new activity and automatically maps metadata traits from the environment to tags.
 

## Parameters

- `name`: <code>string</code>
  The name of the activity.
- `flow`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></code>
  The flow to trace.

## Returns

A flow that executes within the activity span.

