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

## Live Direction

The current focus is on finalizing the granular API reference and ensuring the docs site navigation mirrors the public surface exactly.

- every public API has its own dedicated page
- side-menu entries are visible for every page
- narrative guides stay aligned with the granular page structure
- legacy aliases and terminology are removed in favor of the graph-based model

## Guard Direction

The next API-shape iteration is replacing tuple-based smart binds with an explicit `Guard` concept that is visible in call sites and still binds cleanly in computation expressions.

This direction is only about the source/code surface for the next implementation step. Documentation changes belong to the docs task, not this one.

The contract is:

- `Check` remains the reusable predicate algebra
- `Guard.Of` bridges check-like or absence-like sources into a typed error result/flow
- `Guard.MapError` remaps sources that already carry an error into the target error type
- builders bind the resulting `Result`, `Async<Result>`, `Task<Result>`, `Flow`, `AsyncFlow`, and `TaskFlow` shapes directly
- tuple markers such as `source, orFailTo error` and `source, orMapError mapper` are removed once the `Guard` constructors cover the same behavior

The initial implementation should keep the public intention explicit, not introduce a second predicate DSL, and avoid reusing tuple syntax:

- `Check.notBlank name |> Guard.Of InvalidName`
- `isEnabled |> Guard.Of Disabled`
- `tryGetUser username |> Guard.MapError AuthError`

Implementation acceptance for task 1:

- the builders accept `Guard.Of` and `Guard.MapError` directly
- the old tuple smart-bind overloads are gone from `Flow`, `AsyncFlow`, `TaskFlow`, and `Validate`
- tests exercise plain, async, and task sources through `Guard`
- no user-facing docs are updated yet in this task
- the code compiles and `dotnet test` passes before the task is committed

The implementation sequence should be:

1. add the `Guard` constructors and overloads in source
2. remove the old tuple smart-bind overloads from the builders
3. update tests to exercise `Guard` in `flow`, `asyncFlow`, and `taskFlow`
4. update the narrative guides and API reference to explain `Guard` as the central bridge between `Check` and the flow families
5. normalize the remaining fallback APIs so `orElse` / `orElseWith` consistently mean same-family alternates, not check bridging
6. rename check bridging to `Check.orError` and remove the obsolete `Check.orElse` / `Check.orElseWith` bridge meaning

## Done Means

- the docs read like product documentation for the user
- the API reference is useful without opening the source
- every public API is reachable from the side menu
- semantic edge cases are documented and tested
- the project feels like a maintained library, not a design notebook
