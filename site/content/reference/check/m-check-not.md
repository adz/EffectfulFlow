---
title: "Check."
linkTitle: "``not``"
type: docs
---

<div class="fsdocs-usage">
<code><span>``not``&#32;<span>check</span></span></code>
</div>

Returns success when the supplied check fails.

## Remarks


 This is a logical "not" operation for checks. Note that it discards the success value
 and returns <a href="https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-unit">Unit</a> on success.
 

## Parameters

- `check`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>&lt;'value&gt;</span></code>
  The source <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a> to invert.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a> that succeeds if the input fails.

