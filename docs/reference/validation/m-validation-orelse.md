---
title: "Validation.orElse"
linkTitle: "orElse"
---

<div class="fsdocs-usage">
<code><span>Validation.orElse&#32;<span>fallback&#32;validation</span></span></code>
</div>

Falls back to another validation when the source validation fails.

## Remarks


 This is a left-biased choice operator. If the source succeeds, the fallback is not used.
 If the source fails, the fallback validation is returned as-is.
 

## Parameters

- `fallback`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The validation to use when the source fails.
- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The source validation.

## Returns

The source validation when it succeeds, otherwise the fallback validation.

