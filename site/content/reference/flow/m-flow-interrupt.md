---
title: "Flow.interrupt"
linkTitle: "interrupt"
type: docs
---

<div class="fsdocs-usage">
<code><span>interrupt&#32;<span>fiber</span></span></code>
</div>

Signals a fiber to stop and waits for it to finish its cleanup.

## Parameters

- `fiber`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-fiber-2.html">Fiber</a>&lt;<span>'error,&#32;'value</span>&gt;</span></code>
  The fiber to interrupt.

## Returns

A flow that completes with the fiber's final outcome after interruption.

