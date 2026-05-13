---
title: "Flow.read"
linkTitle: "read"
---

<div class="fsdocs-usage">
<code><span>read&#32;<span>projection</span></span></code>
</div>

Projects a value from the current environment.

## Remarks


 This is the primary way to access dependencies or configuration stored in the environment.
 The <span class="fsdocs-param-name">projection</span> function is applied to the environment at execution time.
 

## Parameters

- `projection`: <code><span>'env&#32;->&#32;'value</span></code>
  A function that extracts a value from the environment.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> containing the projected value.

