---
title: "Validation.bind"
linkTitle: "bind"
---

<div class="fsdocs-usage">
<code><span>Validation.bind&#32;<span>binder&#32;validation</span></span></code>
</div>

Sequences a validation-producing continuation.

## Remarks


 This is the monadic "bind" for validation. Note that this operation short-circuits
 and does not accumulate errors from the binder if the source has already failed.
 For accumulation, use <a href="https://learn.microsoft.com/dotnet/api/map2">map2</a> or the applicative <code>and!</code> syntax.
 

## Parameters

- `binder`: <code><span>'value&#32;->&#32;<span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'next,&#32;'error</span>&gt;</span></span></code>
  A function of type <code>&#39;value -&gt; Validation&lt;&#39;next, &#39;error&gt;</code>.
- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The source validation.

## Returns

The result of the binder or the original diagnostics.

