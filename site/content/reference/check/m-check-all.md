---
title: "Check.all"
linkTitle: "all"
type: docs
---

<div class="fsdocs-usage">
<code><span>all&#32;<span>checks</span></span></code>
</div>

Returns success when every check in the sequence succeeds.

## Remarks


 Sequentially evaluates each check in the <span class="fsdocs-param-name">checks</span> sequence.
 Stops at the first failure.
 

## Parameters

- `checks`: <code><span><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>&lt;'value&gt;</span>&#32;seq</span></code>
  A sequence of checks.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a> that succeeds only if all inputs succeed.

