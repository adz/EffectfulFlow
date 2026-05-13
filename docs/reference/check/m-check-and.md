---
title: "Check."
linkTitle: "``and``"
---

<div class="fsdocs-usage">
<code><span>``and``&#32;<span>left&#32;right</span></span></code>
</div>

Returns success when both checks succeed.

## Remarks


 This is a logical "and" operation. It short-circuits: if <span class="fsdocs-param-name">left</span> fails,
 <span class="fsdocs-param-name">right</span> is not evaluated.
 

## Parameters

- `left`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>&lt;'left&gt;</span></code>
  The first check.
- `right`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>&lt;'right&gt;</span></code>
  The second check.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a> that succeeds only if both inputs succeed.

