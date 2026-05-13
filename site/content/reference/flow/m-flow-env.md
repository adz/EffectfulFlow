---
title: "Flow.env"
linkTitle: "env"
type: docs
---

<div class="fsdocs-usage">
<code><span>env&#32;<span></span></span></code>
</div>

Reads the current environment as the flow value.

## Remarks


 Use this when the entire environment object is needed for the next step of the workflow.
 For projecting specific properties, <a href="https://learn.microsoft.com/dotnet/api/read">read</a> is generally more ergonomic.
 

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> whose successful value is the current environment.

