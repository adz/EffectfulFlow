---
title: "Validation.map"
linkTitle: "map"
---

<div class="fsdocs-usage">
<code><span>Validation.map&#32;<span>mapper&#32;validation</span></span></code>
</div>

Maps the successful value of a validation.

## Parameters

- `mapper`: <code><span>'value&#32;->&#32;'next</span></code>
  A function of type <code>&#39;value -&gt; &#39;next</code>.
- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The source <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>.

## Returns

A validation with the transformed success value.

