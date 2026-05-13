---
title: "Check.fromPredicate"
linkTitle: "fromPredicate"
---

<div class="fsdocs-usage">
<code><span>fromPredicate&#32;<span>predicate&#32;value</span></span></code>
</div>

Builds a check from a predicate while preserving the successful value.

## Parameters

- `predicate`: <code><span>'value&#32;->&#32;bool</span></code>
  A function of type <code>&#39;value -&gt; bool</code> to test the value.
- `value`: <code>'value</code>
  The value of type <code>&#39;value</code> to check.

## Returns

A <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-check-1.html">Check</a> containing the value if the predicate succeeds.

