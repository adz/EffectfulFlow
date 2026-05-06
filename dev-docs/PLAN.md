# FsFlow Plan

This file tracks live product-shape direction and any remaining open questions.
`dev-docs/TASKS.md` is the executable backlog.
`dev-docs/decisions/README.md` indexes the settled decisions that no longer belong here.

## North Star

FsFlow should be framed as one model for Result-based programs in F#.

Start with validation and plain `Result`, then lift the same logic into `Flow`, `AsyncFlow`, or `TaskFlow` when you need environment, async, task, cancellation, logging, or runtime concerns.

The core progression is:

```text
Check -> Result -> Validation -> Flow -> AsyncFlow -> TaskFlow
```

The same predicate and validation vocabulary should work at every step. The user should not need separate helper worlds for raw checks, fail-fast `Result`, accumulated `Validation`, `Async<Result>`, and `Task<Result>`.

## Settled Decisions

These items are no longer live design questions and are tracked in the decision log:

- [Flow architecture](decisions/flow-architecture.md): workflow family split, namespace continuity, cold/restartable semantics, `ColdTask`, and builder surface
- [TaskFlow and ValueTask](decisions/taskflow-valuetask.md): why `TaskFlow` stays Task-backed and why `ValueTask` stays a boundary shape
- [Validation surface](decisions/validation-surface.md): `Check`, `Diagnostics`, `Validation`, and the applicative `validate {}` split
- [API reference page shape](decisions/reference-page-shape.md): one page per public API surface, side-menu entries, examples, and source links
- [Docs source extraction](decisions/docs-source-extraction.md): source-aware API pages with links back to the implementation
- [Reader-env `yield`](decisions/reader-env-yield.md): `yield _.Field` as shorthand while keeping `Flow.read`
- [Option and ValueOption binding](decisions/option-valueoption-binding.md): keep implicit binding only for `unit` error workflows and use explicit conversion helpers for typed errors
- [Logging ergonomics](decisions/logging-ergonomics.md): keep the core logging abstraction generic and treat `ILogger` as an adapter
- [Validation path scoping](decisions/validation-path-scoping.md): `validate {}` stays root-local while scoped helpers produce `Key` / `Index` / `Name` branches for nested diagnostics

## Live Direction

The current focus is making the validation graph usable in real user code.

- keep `Diagnostics<'error>` as the explicit tree-shaped graph type
- keep `Local` as diagnostics attached to the current node and `Children` as nested branches
- add scoped validation helpers so users can write branch-aware validation without manually constructing `Diagnostics`
- prefer a surface like `validate.key`, `validate.index`, and `validate.name` (or equivalent scoped helpers) that prefixes diagnostics produced by a sub-validation block
- keep `validate {}` itself root-local so sibling failures accumulate at the current node
- update the narrative guides and API reference to explain how branch scopes produce `Key` / `Index` / `Name` paths and how `Diagnostics.flatten` reports them
- avoid using hand-built `Diagnostics` trees in user-facing examples except when the tree type itself is the point being documented

## Done Means

- the docs read like product documentation for the user
- the API reference is useful without opening the source
- every public API is reachable from the side menu
- semantic edge cases are documented and tested
- the project feels like a maintained library, not a design notebook
