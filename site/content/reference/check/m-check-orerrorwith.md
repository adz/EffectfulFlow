---
title: "Check.orErrorWith"
linkTitle: "orErrorWith"
type: docs
---

<div class="fsdocs-usage">
<code><span>orErrorWith&#32;<span>errorFn&#32;result</span></span></code>
</div>

Maps a unit error into an application error produced on demand.

## Parameters

- `errorFn`: <code><span>unit&#32;->&#32;'error</span></code>
  A function of type <code>unit -&gt; &#39;error</code> to produce the error.
- `result`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>&lt;'value&gt;</span></code>
  The source <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>.

## Returns

A <a href="https://learn.microsoft.com/dotnet/api/system.result-2">Result</a> with the produced error value.

