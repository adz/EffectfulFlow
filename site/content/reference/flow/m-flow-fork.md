---
title: "Flow.fork"
linkTitle: "fork"
type: docs
---

<div class="fsdocs-usage">
<code><span>fork&#32;<span>flow</span></span></code>
</div>

Starts a flow in a new fiber without waiting for it to complete.

## Parameters

- `flow`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></code>
  The flow to fork.

## Returns

A flow that produces a <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-fiber-2.html">Fiber</a> handle.

