---
title: "Validation"
linkTitle: "Validation<value, error>"
type: docs
---


 An accumulating validation result that keeps the structured diagnostics graph visible.
 

## Remarks


 Unlike <a href="https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-fsharpresult-2">FSharpResult</a>, this type is designed for applicative
 composition using <code>and!</code> in the <code>validate { }</code> builder, which merges errors instead of
 short-circuiting.
 
