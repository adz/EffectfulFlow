---
title: "Validation.merge"
linkTitle: "merge"
---

<div class="fsdocs-usage">
<code><span>Validation.merge&#32;<span>left&#32;right</span></span></code>
</div>

Merges two validations into a validation of a tuple.

## Parameters

- `left`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The first validation.
- `right`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'next,&#32;'error</span>&gt;</span></code>
  The second validation.

## Returns

A validation containing a tuple of the results.

