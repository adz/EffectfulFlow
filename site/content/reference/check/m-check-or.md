---
title: "Check."
linkTitle: "``or``"
type: docs
---

<div class="fsdocs-usage">
<code><span>``or``&#32;<span>left&#32;right</span></span></code>
</div>

Returns success when either check succeeds.

## Remarks


 This is a logical &quot;or&quot; operation. It short-circuits: if <span class="fsdocs-param-name">left</span> succeeds,
 <span class="fsdocs-param-name">right</span> is not evaluated.
 

## Parameters

- `left`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>&lt;'left&gt;</span></code>
  The first check.
- `right`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>&lt;'right&gt;</span></code>
  The second check.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a> that succeeds if either input succeeds.

