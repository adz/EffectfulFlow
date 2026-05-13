---
title: "Flow.bind"
linkTitle: "bind"
---

<div class="fsdocs-usage">
<code><span>bind&#32;<span>binder&#32;flow</span></span></code>
</div>

Sequences a synchronous continuation after a successful value.

## Remarks


 This is the "flatmap" operation for <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>. It allows for dependent
 steps where the second flow depends on the value produced by the first.
 

## Parameters

- `binder`: <code><span>'value&#32;->&#32;<span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'next</span>&gt;</span></span></code>
  A function that takes the successful value and returns a new flow.
- `flow`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></code>
  The source flow to sequence.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> representing the combined workflow.

