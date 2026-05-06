---
title: at
description: API reference for Validation.at
---

# at

Prefixes every diagnostic in a validation with the supplied path segments.


```fsharp
let at (path: PathSegment list) (validation: Validation<'value, 'error>) : Validation<'value, 'error>
```




## Parameters

- `path`: The path segments to prepend.
- `validation`: The validation to scope.

## Returns

A validation with all diagnostics shifted under the given path.

## Information

- **Module**: `Validation`
- **Source**: [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L366)

