# FsFlow Tasks

This file is the active queue for `scripts/ralph-loop-tasks.sh`.
Keep completed work out of this file.
Keep settled design decisions in `dev-docs/decisions/`.
Keep live product and architecture direction in `dev-docs/PLAN.md`.

## Priority Order

Work this queue from top to bottom.

The first four items are the FsFlow Foundation. They take precedence over everything else.

1. [x] Implement the Runtime Registry as the internal storage engine.
   - Introduce a `ServiceKey` that can distinguish by CLR type and optional tag.
   - Store services in a runtime-owned registry rather than in workflow code.
   - Support replacement and lookup operations without exposing the registry as the public API.
   - Keep the registry internal unless a test helper or boundary adapter needs direct access.
   - Target API sketch:
     ```fsharp
     type ServiceKey = { Type: Type; Tag: string option }
     type Registry =
         { Services: Map<ServiceKey, obj>
           Scope: Scope }
     module Registry =
         val empty : Scope -> Registry
         val add<'T> : ?tag:string -> 'T -> Registry -> Registry
         val tryGet<'T> : ?tag:string -> Registry -> 'T option
         val replace<'T> : ?tag:string -> 'T -> Registry -> Registry
     ```
   - Tests to add:
     - inserting and retrieving a service by type
     - inserting two services of the same type with different tags
     - replacing one tag does not affect the other tag
     - missing lookup is handled intentionally
   - Target file/module home:
     - `src/FsFlow/RuntimeRegistry.fs`
     - `src/FsFlow/Runtime.fs` for the carrier-facing helpers

2. [x] Implement `Scope` as the lifecycle owner for resource acquisition and release.
   - Make scope registration explicit.
   - Ensure finalizers run exactly once.
   - Define teardown order and verify it with tests.
   - Make scope usable by layers and other resource-producing helpers.
   - Target behavior:
     ```fsharp
     type Scope() =
         member _.AddFinalizer : (unit -> unit) -> unit
     ```
   - Tests to add:
     - multiple finalizers run after workflow completion
     - finalizers run on success, typed failure, interruption, and defect
     - finalizers run once only
     - teardown order is deterministic
   - Target file/module home:
     - `src/FsFlow/RuntimeScope.fs`

3. [x] Define the public nominal contracts for runtime and app capabilities.
   - Model runtime concerns with named interfaces such as logging, telemetry, clock, and host integration.
   - Model app concerns with named interfaces for repositories, domain services, and feature-specific dependencies.
   - Use interface inheritance or generated bundle interfaces for grouped caps.
   - Keep the public surface readable enough that signatures explain themselves.
   - Target API sketch:
     ```fsharp
     type ILog = abstract Info : string -> unit
     type IClock = abstract UtcNow : unit -> DateTimeOffset
     type IDb = abstract Query : string -> string

     type IRuntimeCaps =
         abstract Log : ILog
         abstract Clock : IClock

     type IAppCaps =
         abstract Db : IDb
     ```
   - Tests to add:
     - a concrete env implements a grouped contract
     - a workflow typed against a bundle contract can run against the concrete env
     - the compiler error for a missing member is readable and points at the contract
   - Target file/module home:
     - `src/FsFlow.Capabilities.Core/Core.fs`
     - `src/FsFlow.Capabilities.Console/Console.fs`
     - `src/FsFlow.Capabilities.FileSystem/FileSystem.fs`
     - `src/FsFlow.Capabilities.Http/Http.fs`
     - `src/FsFlow.Capabilities.Process/Process.fs`

4. [x] Build the adapter layer that projects a registry into a nominal contract.
   - Write the first adapter by hand if needed, but make the final shape generator-friendly.
   - Resolve tags in the adapter, not in user workflows.
   - Hide service lookup mechanics behind a stable bridge boundary.
   - Add tests proving that the same registry can be viewed through multiple contract shapes.
   - Target adapter sketch:
     ```fsharp
     module AppEnvAdapter =
         val fromRegistry : Registry -> AppEnv
     ```
   - Tests to add:
     - one registry can adapt to a runtime-only contract
     - the same registry can adapt to an app-only contract
     - tagged lookup is applied only in the adapter
     - missing tagged services fail at the adapter boundary, not in workflow code
   - Target file/module home:
     - `src/FsFlow/RuntimeAdapter.fs`
     - generator inputs if a source generator is added later

5. [x] Introduce a concrete runtime/app composition root.
   - Decide whether the main execution carrier remains `RuntimeContext<'runtime,'env>` or becomes an equivalent two-part carrier.
   - Keep runtime concerns and app concerns separate in the carrier.
   - Preserve the ability to use runtime caps and app caps together in one workflow.
   - Ensure the composition root can be built once and reused across runs.
   - Target shape:
     ```fsharp
     type RuntimeContext<'runtime, 'env> =
         { Runtime: 'runtime
           Environment: 'env
           CancellationToken: CancellationToken }
     ```
   - Tests to add:
     - runtime-only access works
     - env-only access works
     - both halves can be used in the same workflow
     - the runtime/app split remains visible in the resulting type
   - Target file/module home:
     - `src/FsFlow/Runtime.fs`
     - `src/FsFlow.Hosting/Hosting.fs`

6. [x] Add first-class support for tagged services and local overrides.
   - Allow multiple services of the same CLR type to coexist under different tags.
   - Add override behavior that can replace a service for a subtree or nested composition.
   - Verify that overrides do not mutate unrelated branches.
   - Document the intended precedence rules for tagged lookups and overrides.
   - Examples to support:
     ```fsharp
     reg.Add<ILog>(mainLog, tag = Some "Main")
     reg.Add<ILog>(auditLog, tag = Some "Audit")
     ```
   - Tests to add:
     - `Main` and `Audit` tags resolve to different values
     - subtree override affects only the subtree
     - untagged lookups do not accidentally pick tagged values
   - Target file/module home:
     - `src/FsFlow/RuntimeRegistry.fs`
     - `src/FsFlow/RuntimeLayer.fs`

7. [x] Add layer/provisioner support on top of the foundation.
   - Define how a layer creates or transforms runtime state.
   - Ensure layers cooperate with scope and finalizers.
   - Support combining layers horizontally and chaining them vertically.
   - Keep layer composition separate from ordinary workflow code.
   - Target idea:
     ```fsharp
     type Layer<'input, 'error, 'output> =
         Registry -> CancellationToken -> Effect<'output, 'error>
     ```
   - Tests to add:
     - a layer can produce a service and register a finalizer
     - two layers can be combined
     - a layer can feed its output into another layer
     - the layer path does not require user workflows to manipulate the registry directly
   - Target file/module home:
     - `src/FsFlow/RuntimeLayer.fs`

8. [x] Preserve the existing `Exit` / `Cause` execution model while the foundation lands.
   - Keep `Cause.Fail`, `Cause.Interrupt`, and `Cause.Die` distinct.
   - Make `Flow.run`, `AsyncFlow.run`, and `TaskFlow.run` preserve the cause model.
   - Avoid collapsing defects into typed failures.
   - Make sure the new architecture does not regress current runtime semantics.
   - Non-goals:
     - do not flatten defects into `Result`
     - do not let registry or adapter errors silently become domain errors
     - do not lose cancellation identity
   - Tests to keep green:
     - typed failures remain typed failures
     - cancellations remain interruptions
     - defects remain defects
   - Target file/module home:
     - `src/FsFlow/Core.fs`
     - `src/FsFlow/Flow.fs`
     - `src/FsFlow/AsyncFlow.fs`
     - `src/FsFlow/TaskFlow.fs`

9. [x] Update the docs and generated reference after the foundation is implemented.
   - Rewrite the public docs around the registry/contract/adapter/scope model.
   - Remove stale tuple-SRTP language from user-facing material.
   - Keep generated reference pages aligned with the implementation.
   - Ensure examples show the nominal contract approach rather than the old debate.
   - Required doc outputs:
     - clear end-to-end workflow example
     - tagged-service example
     - scope/lifecycle example
     - runtime/app split example
     - adapter example
   - Target file/module home:
     - `dev-docs/PLAN.md`
     - `README.md`
     - generated reference pages under `docs/reference/**`

10. [x] Evolve STM after the foundation is stable.
    - Keep the STM backlog separate from the environment redesign.
    - Move from the pessimistic lock model to `retry` / `orElse` style coordination.
    - Preserve the same runtime discipline as the rest of the library.
    - Do not start on STM until items 1 through 9 are in a usable state.
    - Target file/module home:
      - `src/FsFlow/Stm.fs`

## Acceptance Checks

The Foundation work is complete when the following are true:

- a user can define a nominal capability interface and consume it from a workflow without seeing registry internals
- a runtime registry can host tagged services and resolve them through an adapter
- scope cleanup runs deterministically and is covered by tests
- local override behavior is explicit and tested
- public workflows remain readable and do not expose SRTP details
- docs describe the final architecture, not the exploration path
