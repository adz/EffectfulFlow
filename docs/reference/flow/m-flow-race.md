---
title: "Flow.race"
linkTitle: "race"
---

<div class="fsdocs-usage">
<code><span>race&#32;<span>left&#32;right</span></span></code>
</div>

Runs two flows concurrently and returns the result of the first one to complete.

## Remarks


 The &quot;loser&quot; flow is interrupted immediately.
 

## Parameters

- `left`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></code>
- `right`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></code>

