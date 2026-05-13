---
title: "Validation.key"
linkTitle: "key"
type: docs
---

<div class="fsdocs-usage">
<code><span>Validation.key&#32;<span>key&#32;validation</span></span></code>
</div>

Prefixes a validation with a keyed branch.

## Parameters

- `key`: <code>string</code>
  The branch key.
- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The validation to scope.

## Returns

A validation whose diagnostics are prefixed with <code>Key key</code>.

