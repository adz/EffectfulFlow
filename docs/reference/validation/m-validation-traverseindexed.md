---
title: "Validation.traverseIndexed"
linkTitle: "traverseIndexed"
---

<div class="fsdocs-usage">
<code><span>Validation.traverseIndexed&#32;<span>binder&#32;values</span></span></code>
</div>

Maps a sequence into validations while prefixing each item with its index.

## Remarks


 This is the indexed version of <a href="https://learn.microsoft.com/dotnet/api/sequence">sequence</a>. It is useful for list and array
 validation because each item can keep its own <a href="https://learn.microsoft.com/dotnet/api/fsflow.pathsegment.index">Index</a>
 branch without the caller manually wrapping every item.
 

## Parameters

- `binder`: <code><span>int&#32;->&#32;'source&#32;->&#32;<span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></span></code>
  A function of type <code>int -&gt; &#39;source -&gt; Validation&lt;&#39;value, &#39;error&gt;</code>.
- `values`: <code><span>'source&#32;seq</span></code>
  The input sequence.

## Returns

A validation containing the list of values or accumulated diagnostics.

