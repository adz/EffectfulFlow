---
title: "Validation"
weight: 60
type: docs
---

This page shows the source-documented `Validation` surface: the accumulating result type, module functions, and path-scoping helpers.

## Core type

- [`Validation`](./t-validation-2.md): 
 An accumulating validation result that keeps the structured diagnostics graph visible.
 

## Module functions

- [`Validation.toResult`](./m-validation-toresult.md): Converts a <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a> into a standard <a href="https://learn.microsoft.com/dotnet/api/system.result-2">Result</a>.
- [`Validation.ok`](./m-validation-ok.md): Creates a successful validation result.
- [`Validation.error`](./m-validation-error.md): Creates a failing validation result with the provided diagnostics.
- [`Validation.succeed`](./m-validation-succeed.md): Alias for <a href="https://learn.microsoft.com/dotnet/api/ok">ok</a>.
- [`Validation.fail`](./m-validation-fail.md): Alias for <a href="https://learn.microsoft.com/dotnet/api/error">error</a>.
- [`Validation.fromResult`](./m-validation-fromresult.md): Lifts a standard <a href="https://learn.microsoft.com/dotnet/api/system.result-2">Result</a> into the <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-validation-2.html">Validation</a> context.
- [`Validation.map`](./m-validation-map.md): Maps the successful value of a validation.
- [`Validation.bind`](./m-validation-bind.md): Sequences a validation-producing continuation.
- [`Validation.mapError`](./m-validation-maperror.md): Maps the error type of a validation graph.
- [`Validation.map2`](./m-validation-map2.md): Combines two validations, accumulating errors if both fail.
- [`Validation.map3`](./m-validation-map3.md): Combines three validations, accumulating errors when any input fails.
- [`Validation.apply`](./m-validation-apply.md): Applies a validation-wrapped function to a validation-wrapped value.
- [`Validation.ignore`](./m-validation-ignore.md): Maps a successful validation value to <code>unit</code> while preserving the diagnostics.
- [`Validation.orElse`](./m-validation-orelse.md): Falls back to another validation when the source validation fails.
- [`Validation.orElseWith`](./m-validation-orelsewith.md): Computes a fallback validation from the source diagnostics when validation fails.
- [`Validation.collect`](./m-validation-collect.md): Collects a sequence of validations into a single validation of a list.
- [`Validation.sequence`](./m-validation-sequence.md): Transforms a sequence of validations into a validation of a list.
- [`Validation.traverseIndexed`](./m-validation-traverseindexed.md): Maps a sequence into validations while prefixing each item with its index.
- [`Validation.merge`](./m-validation-merge.md): Merges two validations into a validation of a tuple.

## Path scoping

- [`Validation.at`](./m-validation-at.md): Scopes a validation under the supplied path segments.
- [`Validation.key`](./m-validation-key.md): Prefixes a validation with a keyed branch.
- [`Validation.index`](./m-validation-index.md): Prefixes a validation with an indexed branch.
- [`Validation.name`](./m-validation-name.md): Prefixes a validation with a named branch.

