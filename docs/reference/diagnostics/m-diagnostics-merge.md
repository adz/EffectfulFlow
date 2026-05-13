---
title: "Diagnostics.merge"
linkTitle: "merge"
---

<div class="fsdocs-usage">
<code><span>Diagnostics.merge&#32;<span>left&#32;right</span></span></code>
</div>

Recursively merges two diagnostics graphs, combining shared branches and local errors.

## Remarks


 This is the core operation for applicative validation. It ensures that errors from sibling
 fields are collected together into a single structured graph.
 

## Parameters

- `left`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostics-1.html">Diagnostics</a>&lt;'error&gt;</span></code>
  The first graph of type <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostics-1.html">Diagnostics</a>.
- `right`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostics-1.html">Diagnostics</a>&lt;'error&gt;</span></code>
  The second graph of type <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostics-1.html">Diagnostics</a>.

## Returns

A new <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostics-1.html">Diagnostics</a> containing the union of both inputs.

