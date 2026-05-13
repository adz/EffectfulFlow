---
title: "Flow.orElseWith"
linkTitle: "orElseWith"
type: docs
---

<div class="fsdocs-usage">
<code><span>orElseWith&#32;<span>fallback&#32;flow</span></span></code>
</div>

Falls back to another flow when the source flow fails.Computes a fallback flow from the source error when the source flow fails.

## Parameters

- `fallback`: <code><span>'error&#32;->&#32;<span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></span></code>
- `flow`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></code>

