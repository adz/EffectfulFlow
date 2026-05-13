---
title: "Check.any"
linkTitle: "any"
---

<div class="fsdocs-usage">
<code><span>any&#32;<span>checks</span></span></code>
</div>

Returns success when at least one check in the sequence succeeds.

## Remarks


 Sequentially evaluates each check in the <span class="fsdocs-param-name">checks</span> sequence.
 Stops at the first success.
 

## Parameters

- `checks`: <code><span><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a>&lt;'value&gt;</span>&#32;seq</span></code>
  A sequence of checks.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a> that succeeds if any input succeeds.

