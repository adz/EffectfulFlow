---
title: "Check.orError"
linkTitle: "orError"
---

<div class="fsdocs-usage">
<code><span>orError&#32;<span>error&#32;result</span></span></code>
</div>

Maps a unit error into the supplied application error value.

## Remarks


 This is the primary bridge from checks to domain-specific results.
 

## Parameters

- `error`: <code>'error</code>
  The domain error of type <code>&#39;error</code> to return on failure.
- `result`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>&lt;'value&gt;</span></code>
  The source <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>.

## Returns

A <a href="https://learn.microsoft.com/dotnet/api/system.result-2">Result</a> with the provided error value.

