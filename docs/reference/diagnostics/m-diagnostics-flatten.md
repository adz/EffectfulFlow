---
title: "Diagnostics.flatten"
linkTitle: "flatten"
---

<div class="fsdocs-usage">
<code><span>Diagnostics.flatten&#32;<span>graph</span></span></code>
</div>

Flattens the structured diagnostics graph into a linear list of diagnostics.

## Remarks


 During flattening, child paths are accumulated from the root down into each emitted diagnostic.
 The tree itself stores only local errors and child branches, while <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostic-1.html">Diagnostic</a>
 is reserved for reporting output.
 

## Parameters

- `graph`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostics-1.html">Diagnostics</a>&lt;'error&gt;</span></code>
  The <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostics-1.html">Diagnostics</a> to flatten.

## Returns

A list of type <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostic-1.html">Diagnostic</a> list.

