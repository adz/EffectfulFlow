---
title: "Validation.ignore"
linkTitle: "ignore"
---

<div class="fsdocs-usage">
<code><span>Validation.ignore&#32;<span>validation</span></span></code>
</div>

Maps a successful validation value to <code>unit</code> while preserving the diagnostics.

## Parameters

- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The source validation.

## Returns

A validation that keeps the original diagnostics and discards the success value.

