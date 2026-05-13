---
title: "Validation.map3"
linkTitle: "map3"
---

<div class="fsdocs-usage">
<code><span>Validation.map3&#32;<span>mapper&#32;left&#32;middle&#32;right</span></span></code>
</div>

Combines three validations, accumulating errors when any input fails.

## Parameters

- `mapper`: <code><span>'left&#32;->&#32;'middle&#32;->&#32;'right&#32;->&#32;'value</span></code>
  A function of type <code>&#39;left -&gt; &#39;middle -&gt; &#39;right -&gt; &#39;value</code>.
- `left`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'left,&#32;'error</span>&gt;</span></code>
  The first validation.
- `middle`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'middle,&#32;'error</span>&gt;</span></code>
  The second validation.
- `right`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'right,&#32;'error</span>&gt;</span></code>
  The third validation.

## Returns

A validation with the combined result.

