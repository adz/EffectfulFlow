---
title: "Diagnostics.toString"
linkTitle: "toString"
type: docs
---

<div class="fsdocs-usage">
<code><span>Diagnostics.toString&#32;<span>graph</span></span></code>
</div>

Renders a diagnostics graph in a YAML-like layout for display.

## Remarks


 This is intended for human-readable output. Empty sections are omitted, and children are
 shown directly under their branch labels at the same indentation level as errors. Errors
 render as YAML-style bullet items without an `Errors:` key. Use
 <a href="https://learn.microsoft.com/dotnet/api/fsflow.diagnostics.flatten">flatten</a> when you need path-bearing diagnostics for
 reporting or assertions.
 

## Parameters

- `graph`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostics-1.html">Diagnostics</a>&lt;'error&gt;</span></code>
  The diagnostics graph to render.

## Returns

A formatted string representation of the graph.

