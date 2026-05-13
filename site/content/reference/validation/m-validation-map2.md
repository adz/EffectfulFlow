---
title: "Validation.map2"
linkTitle: "map2"
type: docs
---

<div class="fsdocs-usage">
<code><span>Validation.map2&#32;<span>mapper&#32;left&#32;right</span></span></code>
</div>

Combines two validations, accumulating errors if both fail.

## Remarks


 This is the core applicative operation. If both <span class="fsdocs-param-name">left</span> and 
 <span class="fsdocs-param-name">right</span> fail, their diagnostics graphs are merged.
 

## Parameters

- `mapper`: <code><span>'left&#32;->&#32;'right&#32;->&#32;'value</span></code>
  A function of type <code>&#39;left -&gt; &#39;right -&gt; &#39;value</code>.
- `left`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'left,&#32;'error</span>&gt;</span></code>
  The first validation.
- `right`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'right,&#32;'error</span>&gt;</span></code>
  The second validation.

## Returns

A validation with the combined result.

