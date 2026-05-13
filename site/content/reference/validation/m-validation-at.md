---
title: "Validation.at"
linkTitle: "at"
type: docs
---

<div class="fsdocs-usage">
<code><span>Validation.at&#32;<span>path&#32;validation</span></span></code>
</div>

Scopes a validation under the supplied path segments.

## Parameters

- `path`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-pathsegment.html">PathSegment</a>&#32;list</span></code>
  The path segments to apply to the validation.
- `validation`: <code><span><a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>
  The validation to scope.

## Returns

A validation nested under the given path.

