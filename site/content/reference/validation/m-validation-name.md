---
title: "Validation.name"
linkTitle: "name"
type: docs
---

<div class="fsdocs-usage">
<code><span>Validation.name&#32;<span>name&#32;validation</span></span></code>
</div>

Prefixes a validation with a named branch.

## Parameters

- `name`: <code>string</code>
  The branch name.
- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The validation to scope.

## Returns

A validation whose diagnostics are prefixed with <code>Name name</code>.

