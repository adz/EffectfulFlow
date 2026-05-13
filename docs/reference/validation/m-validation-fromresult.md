---
title: "Validation.fromResult"
linkTitle: "fromResult"
---

<div class="fsdocs-usage">
<code><span>Validation.fromResult&#32;<span>result</span></span></code>
</div>

Lifts a standard <a href="https://learn.microsoft.com/dotnet/api/system.result-2">Result</a> into the <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a> context.

## Remarks


 If the result is an error, it is wrapped in a root-level <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-diagnostics-1.html">Diagnostics</a> graph.
 

## Parameters

- `result`: <code><span><a href="https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-fsharpresult-2">Result</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The result to lift.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a> mirroring the result.

