---
title: Validation
description: Source-documented accumulating validation for FsFlow.
---

# Validation

This page shows the source-documented `Validation` surface: the accumulating result type, the module functions, the path-scoping helpers, and the `validate { }` builder.

## Core type

- type `Validation`: An accumulating validation result that keeps the structured diagnostics graph visible. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L56)

## Builder

- [`Builders.validate`](./builders-validate.md): The accumulating `validate { }` computation expression. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Builders.fs#L497)

## Module functions

- module `Validation`: Helpers for accumulating validation results with mergeable diagnostics. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L263)
- [`Validation.toResult`](./validation-toresult.md): Converts a `Validation` into a standard `Result`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L269)
- [`Validation.succeed`](./validation-succeed.md): Creates a successful validation result. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L275)
- [`Validation.fail`](./validation-fail.md): Creates a failing validation result with the provided diagnostics. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L281)
- [`Validation.fromResult`](./validation-fromresult.md): Lifts a standard `Result` into the `Validation` context. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L290)
- [`Validation.map`](./validation-map.md): Maps the successful value of a validation. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L299)
- [`Validation.bind`](./validation-bind.md): Sequences a validation-producing continuation. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L314)
- [`Validation.mapError`](./validation-maperror.md): Maps the error type of a validation graph. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L326)
- [`Validation.map2`](./validation-map2.md): Combines two validations, accumulating errors if both fail. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L351)
- [`Validation.apply`](./validation-apply.md): Applies a validation-wrapped function to a validation-wrapped value. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L368)
- [`Validation.collect`](./validation-collect.md): Collects a sequence of validations into a single validation of a list. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L380)
- [`Validation.sequence`](./validation-sequence.md): Transforms a sequence of validations into a validation of a list. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L391)
- [`Validation.traverseIndexed`](./validation-traverseindexed.md): Maps a sequence into validations while prefixing each item with its index. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L447)
- [`Validation.merge`](./validation-merge.md): Merges two validations into a validation of a tuple. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L398)

## Path scoping

- [`Validation.at`](./validation-at.md): Scopes a validation under the supplied path segments. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L405)
- [`Validation.key`](./validation-key.md): Prefixes a validation with a keyed branch. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L421)
- [`Validation.index`](./validation-index.md): Prefixes a validation with an indexed branch. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L428)
- [`Validation.name`](./validation-name.md): Prefixes a validation with a named branch. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L435)

