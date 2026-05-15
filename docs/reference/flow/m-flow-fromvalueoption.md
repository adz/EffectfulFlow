---
title: "Flow.fromValueOption"
linkTitle: "fromValueOption"
---

<div class="fsdocs-usage">
<code><span>fromValueOption&#32;<span>error&#32;value</span></span></code>
</div>

Lifts a value option into a synchronous flow with the supplied error.

## Parameters

- `error`: <code>'error</code>
  The error to return if the value option is <a href="https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-fsharpvalueoption-1-valuenone">ValueNone</a>.
- `value`: <code><span>'value&#32;voption</span></code>
  The value option to lift.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> that succeeds with the option&#39;s value or fails with the provided error.

