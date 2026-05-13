---
title: "Validation.mapError"
linkTitle: "mapError"
type: docs
---

<div class="fsdocs-usage">
<code><span>Validation.mapError&#32;<span>mapper&#32;validation</span></span></code>
</div>

Maps the error type of a validation graph.

## Parameters

- `mapper`: <code><span>'error&#32;->&#32;'nextError</span></code>
  A function of type <code>&#39;error -&gt; &#39;nextError</code>.
- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The source <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>.

## Returns

A validation with transformed error values.

