---
title: "Validation.toResult"
linkTitle: "toResult"
---

<div class="fsdocs-usage">
<code><span>Validation.toResult&#32;<span>validation</span></span></code>
</div>

Converts a <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a> into a standard <a href="https://learn.microsoft.com/dotnet/api/system.result-2">Result</a>.

## Parameters

- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The validation to convert.

## Returns

A result containing either the success value or the full diagnostics graph.

