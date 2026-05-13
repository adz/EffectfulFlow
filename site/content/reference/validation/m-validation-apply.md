---
title: "Validation.apply"
linkTitle: "apply"
type: docs
---

<div class="fsdocs-usage">
<code><span>Validation.apply&#32;<span>validation&#32;value</span></span></code>
</div>

Applies a validation-wrapped function to a validation-wrapped value.

## Parameters

- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span><span>(<span>'value&#32;->&#32;'next</span>)</span>,&#32;'error</span>&gt;</span></code>
  The validation containing the function.
- `value`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The validation containing the value.

## Returns

The result of applying the function to the value, with accumulated errors.

