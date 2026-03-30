# Effect.FS Plan

## Purpose

Build an F# effect library with excellent developer experience and excellent .NET interop.

The benchmark for usability is not academic purity. The benchmark is whether ordinary F# application code becomes easier to write, easier to read, and easier to evolve than with today's common `Async<Result<_,_>>` and FsToolkit-style workflows.

This project is:

- general-purpose
- F#-first
- strongly interoperable with .NET
- focused on effect handling, dependency access, logging, and related application concerns

This project is not:

- an excuse to build a broad runtime before the core DX is proven

## Product Direction

The intended shape is:

- a small core effect type
- an ergonomic computation expression
- direct interop with `Result`, `Async`, `Task`, and normal .NET APIs
- first-class environment / dependency access
- a path for logging and other effects without hidden DI containers
- an optional compatibility story for FsToolkit-style code

The key question is not "can this model represent effects?"

The key question is "does this feel obviously better to use in F#?"

## Primary Goals

1. Better DX than common F# effect patterns

- reduce boilerplate around `Async<Result<_,_>>`
- keep type flow understandable
- avoid combinator soup in normal application code
- make the happy path read clearly in `effect {}`

2. Excellent .NET interoperability

- easy interop with `Task`
- easy interop with `Async`
- straightforward cancellation handling
- easy use from mixed F# / C# codebases
- no awkward barriers around existing libraries

3. Explicit dependency access

- support reading dependencies from environment/context
- make dependencies visible in types
- avoid service locator feel
- avoid heavy OO DI assumptions

4. Practical effect capabilities

- typed failures where useful
- logging and similar capabilities modeled cleanly
- resource safety, timeout, and retry support over time
- predictable execution semantics

## Design Constraints

- Prefer F#-native ergonomics over copying another ecosystem literally.
- Prefer explicitness in the core, even if convenience layers exist on top.
- Keep names intuitive for F# developers.
- Do not force users into a specific application architecture.
- Avoid building a huge runtime until the small core is clearly worthwhile.
- Treat typed errors as modeled, expected failures that callers can reasonably handle.
- Keep exception/defect handling distinct from typed business or infrastructure errors.
- Prefer explicit error translation between layers over shared catch-all error unions.

## Comparison Targets

The most important comparison points are:

- FsToolkit and related F# workflow libraries
- ordinary `Async<Result<_,_>>` code
- .NET `Task`-based application code
- Effect-TS as inspiration for scope and concepts, not as a literal design target

The bar is:

- easier to read than stacked wrappers
- more composable than ad hoc helper modules
- better dependency and effect story than plain FsToolkit
- still natural in normal F# code

## Open Questions

These questions matter now:

1. What is the canonical public effect type?
2. Should the main computation expression stay explicit about lifting, or support more direct binding of `Result`, `Async`, and `Task`?
3. What should the environment API be called so it feels natural in F#?
4. How should typed business errors relate to defects / exceptions?
5. What is the minimum useful logging/dependency story?
6. How far should FsToolkit compatibility go?
7. Which convenience features belong in the core, and which belong in an opt-in compatibility layer?
8. How strongly should the library encourage small layer-local error types and explicit remapping into domain errors?

## Near-Term Plan

### Phase 1: sharpen the core UX

Deliverables:

- settle on clearer names for the core API
- improve the `effect {}` experience
- validate ergonomics with small realistic examples
- document the intended strict vs convenient usage style
- document the intended error-modeling style, especially typed failures vs defects

Exit criteria:

- the examples read like good F# rather than plumbing
- the environment/dependency story feels understandable
- handling `Result`, `Async`, and `Task` no longer feels awkward
- error translation between layers feels explicit and natural rather than ceremonial

### Phase 2: prove dependency and logging ergonomics

Deliverables:

- a small environment/dependency API
- an example showing dependency access in ordinary app code
- a basic logging effect story
- clearer guidance on how this differs from DI containers

Exit criteria:

- app code using dependencies and logging feels simpler than the equivalent FsToolkit-style wiring

### Phase 3: define compatibility strategy

Deliverables:

- explicit position relative to FsToolkit
- decide whether compatibility is partial or exact
- if worth doing, design a separate compatibility layer or builder

Exit criteria:

- compatibility is a deliberate product decision, not an accidental blur

### Phase 4: deepen practical capabilities

Deliverables:

- resource safety story
- cancellation / timeout helpers
- retry helpers
- better examples using real .NET APIs

Exit criteria:

- the library is useful for ordinary application effect management, not just toy examples

## Follow-On Effect Priorities

Once the core UX is credible, the next effects to prioritize are:

1. Environment / dependency access
2. Typed failure and exception boundary clarity
3. `Async` / `Task` interop polish
4. Logging
5. Cancellation
6. Resource safety
7. Timeout
8. Retry
9. Clock / time

These priorities should not flatten all failures into one typed channel.

- Typed errors should model expected failures, not serve as a blanket wrapper over exceptions.
- Lower layers should own small, meaningful error types.
- Higher layers should explicitly map those errors into their own domain language.
- Exception capture should happen at deliberate boundaries where a thrown failure can be classified meaningfully.
- The design should avoid encouraging `Error = Exception` in disguise.

## Error Modeling Position

The library should treat typed failure as part of domain and application modeling, not as a universal replacement for exceptions.

What typed errors are for:

- validation failures
- parsing and decoding failures
- expected remote or persistence failures after deliberate classification
- business rule violations

What typed errors are not for:

- programmer defects
- broken invariants
- indiscriminate wrapping of arbitrary exceptions

The intended style is closer to Elm:

- model failures with small local types
- translate errors explicitly between layers
- keep that translation visible in code

The library should make this style ergonomic by improving operations like `mapError`, explicit boundary capture, and interop helpers that preserve layer-local error meanings.

These are the fastest path to making the library useful in ordinary .NET application code.

Lower priority for now:

- streams
- queues
- metrics/tracing platforms
- large runtime concurrency abstractions
- domain-specific orchestration machinery

## Actor / Mailbox Note

A real actor or mailbox wrapper may be useful later, but it should not be an early project focus.

Reason:

- F# already has `MailboxProcessor`
- wrapping it too early risks building architecture-specific machinery before the core effect API is good enough
- actor support is better treated as an integration or companion layer than as the main identity of the library

If actor support is added later, the goal should be:

- clean interop with `MailboxProcessor`
- explicit boundaries between mailbox logic and effectful operations
- no requirement that application code adopt an actor model just to use the effect library

## Immediate Next Work

The next implementation work should focus on:

- improving naming and ergonomics
- simplifying the examples until they read like a getting-started guide
- deciding how direct `Result`, `Async`, and `Task` handling should be
- clarifying the intended FsToolkit relationship

Do not expand into unrelated runtime machinery until those are solid.
