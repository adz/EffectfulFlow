---
title: "Flow.catch"
linkTitle: "catch"
type: docs
---

<div class="fsdocs-usage">
<code><span>catch&#32;<span>handler&#32;flow</span></span></code>
</div>

Catches exceptions raised during execution and maps them to a typed error.

## Remarks


 Exceptions that are not caught by this helper will bubble up to the caller of <a href="https://learn.microsoft.com/dotnet/api/run">run</a>.
 This ensures that known exceptions can be handled within the flow context.
 

## Parameters

- `handler`: <code><span>exn&#32;->&#32;'error</span></code>
  A function of type <code>exn -&gt; &#39;error</code> to map the exception.
- `flow`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a>&lt;<span>'env,&#32;'error,&#32;'value</span>&gt;</span></code>
  The source flow of type <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> to monitor.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-flow-3.html">Flow</a> that converts exceptions into success-path errors.

