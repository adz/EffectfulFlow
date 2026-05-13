---
title: "Validation.index"
linkTitle: "index"
---

<div class="fsdocs-usage">
<code><span>Validation.index&#32;<span>index&#32;validation</span></span></code>
</div>

Prefixes a validation with an indexed branch.

## Parameters

- `index`: <code>int</code>
  The branch index.
- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The validation to scope.

## Returns

A validation whose diagnostics are prefixed with <code>Index index</code>.

