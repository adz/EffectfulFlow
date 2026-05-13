---
title: "Validation.orElseWith"
linkTitle: "orElseWith"
---

<div class="fsdocs-usage">
<code><span>Validation.orElseWith&#32;<span>fallback&#32;validation</span></span></code>
</div>

Computes a fallback validation from the source diagnostics when validation fails.

## Remarks


 This is the lazy counterpart to <a href="https://learn.microsoft.com/dotnet/api/orelse">orElse</a> and is useful when the alternate
 branch depends on the accumulated diagnostics.
 

## Parameters

- `fallback`: <code><span><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostics-1.html">Diagnostics</a>&lt;'error&gt;</span>&#32;->&#32;<span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></span></code>
  A function that turns the diagnostics into an alternate validation.
- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The source validation.

## Returns

The source validation when it succeeds, otherwise the computed fallback validation.

