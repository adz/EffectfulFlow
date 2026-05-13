---
title: "Diagnostics"
weight: 80
type: docs
---

The `Diagnostics` type represents a structured graph of validation failures.

## Graph types

- [`PathSegment`](./t-pathsegment.md): Location markers used to describe where a diagnostic belongs in a validation graph.
- [`Diagnostic<_>.Path`](./t-path.md): The path to the source of the error.
- [`Diagnostic`](./t-diagnostic-1.md): A single failure item attached to a path in a validation graph.
- [`Diagnostics`](./t-diagnostics-1.md): 
 A mergeable validation graph that carries local errors and nested child branches.
 

## Module functions

- [`Diagnostics.empty`](./m-diagnostics-empty.md): Creates an empty diagnostics graph with no errors.
- [`Diagnostics.singleton`](./m-diagnostics-singleton.md): Creates a diagnostics graph containing exactly one error at the root.
- [`Diagnostics.merge`](./m-diagnostics-merge.md): Recursively merges two diagnostics graphs, combining shared branches and local errors.
- [`Diagnostics.toString`](./m-diagnostics-tostring.md): Renders a diagnostics graph in a YAML-like layout for display.
- [`Diagnostics.flatten`](./m-diagnostics-flatten.md): Flattens the structured diagnostics graph into a linear list of diagnostics.

