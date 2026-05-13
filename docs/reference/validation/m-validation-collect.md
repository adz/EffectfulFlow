---
title: "Validation.collect"
linkTitle: "collect"
---

<div class="fsdocs-usage">
<code><span>Validation.collect&#32;<span>validations</span></span></code>
</div>

Collects a sequence of validations into a single validation of a list.

## Remarks


 This operation is applicative: it will collect errors from ALL items in the sequence.
 

## Parameters

- `validations`: <code><span><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span>&#32;seq</span></code>
  A sequence of type <code>seq&lt;Validation&lt;&#39;value, &#39;error&gt;&gt;</code>.

## Returns

A validation containing the list of values or accumulated diagnostics.

