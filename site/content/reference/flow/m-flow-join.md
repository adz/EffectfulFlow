---
title: "Flow.join"
linkTitle: "join"
type: docs
---

<div class="fsdocs-usage">
<code><span>join&#32;<span>fiber</span></span></code>
</div>

Waits for a fiber to complete and returns its final outcome.

## Parameters

- `fiber`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-fiber-2.html">Fiber</a>&lt;<span>'error,&#32;'value</span>&gt;</span></code>
  The fiber to join.

## Returns

A flow that completes with the fiber's outcome.

