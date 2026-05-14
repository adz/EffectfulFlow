# FsFlow Plan

This file tracks unresolved product direction only.
`dev-docs/decisions/README.md` indexes settled decisions that no longer belong here.

## Current Priority

The top priority is the FsFlow Foundation:

- Runtime Registry for tagged service storage, scope ownership, and local overrides
- Nominal Contracts for the public capability surface
- Adapter Layer for bridging runtime storage to public contracts
- Resource Scope for deterministic acquire/release lifecycle management

Everything else must fit around that foundation.

## Foundation Goals

The foundation is complete only when all of the following are true:

1. Public workflow signatures are readable without exposing runtime internals.
2. Runtime storage can hold multiple services, including tagged services of the same CLR type.
3. Scopes dispose resources deterministically and in the correct order.
4. Local overrides can replace a service for a subtree or nested workflow without rebuilding the entire app.
5. Public contracts remain nominal, stable, and easy to understand in diagnostics.
6. Boilerplate between runtime storage and public contracts is generated or mechanically derived.
7. The existing `Exit` / `Cause` execution semantics remain intact.
8. The docs and generated reference describe the foundation, not an old placeholder model.

If any one of these fails, the foundation is incomplete.

## Architecture

### Proposed File Layout

This is the target layout for the foundation. The exact names can still move, but the responsibilities should not.

```text
src/FsFlow/
  Core.fs                 // Flow, Exit, Cause, Effect, contracts used by core workflows
  Runtime.fs              // RuntimeContext and runtime/app carrier helpers
  RuntimeRegistry.fs      // internal registry, tags, scope, add/get/replace
  RuntimeScope.fs         // scope lifecycle and finalizer handling
  RuntimeAdapter.fs       // registry -> nominal contract bridge
  RuntimeLayer.fs         // layer/provisioning helpers

src/FsFlow.Capabilities.*
  *.fs                    // public capability interfaces and helper functions

src/FsFlow.Generators/    // source generators or codegen inputs, if added
  ...
```

Implementation rule:

- if code is about service lookup, tagging, or lifecycle, it belongs in runtime registry/scope modules
- if code is about what a workflow needs, it belongs in capability modules
- if code is about converting runtime state into public contracts, it belongs in the adapter layer
- if code is about building runtime state, it belongs in layer/provisioner modules

### 1. Runtime Registry

The runtime registry is internal machinery.

Responsibilities:

- store concrete service instances
- distinguish services by type and optional tag
- support replacement and override operations
- own the current scope
- act as the bridge target for adapters

Design rules:

- the registry is not the public API
- the registry is not the thing user workflows program against
- the registry must not leak into ordinary workflow signatures
- the registry must remain capable of hosting multiple values of the same CLR type when tagged

Suggested core shape:

```fsharp
type ServiceKey = { Type: Type; Tag: string option }
type Registry = { Services: Map<ServiceKey, obj>; Scope: Scope }
```

The exact internal representation can change, but the behavior above cannot.

Concrete shape to aim for:

```fsharp
type ServiceKey =
    { Type: Type
      Tag: string option }

type Scope() =
    member _.AddFinalizer : (unit -> unit) -> unit

type Registry =
    { Services: Map<ServiceKey, obj>
      Scope: Scope }

module Registry =
    val empty : Scope -> Registry
    val add<'T> : ?tag:string -> 'T -> Registry -> Registry
    val tryGet<'T> : ?tag:string -> Registry -> 'T option
    val get<'T> : ?tag:string -> Registry -> 'T
    val replace<'T> : ?tag:string -> 'T -> Registry -> Registry
```

Expected runtime behavior:

- `add` inserts a value under `typeof<'T>` and optional tag
- `tryGet` reads without throwing
- `get` throws or fails in a controlled, intentional way when missing
- `replace` overwrites the matching key
- `Scope` owns finalizers and runs them exactly once

The registry must support multiple services of the same CLR type via `Tag`.

### 2. Nominal Contracts

The public API should use standard F# interfaces as named capability contracts.

Responsibilities:

- describe what a workflow needs
- provide readable names in signatures and errors
- group related services into bundles
- support interface inheritance for composition

Design rules:

- the user should see named contracts, not SRTP machinery
- contracts should be ordinary interfaces or interface groups
- contracts should work well with generated adapters
- contracts should support runtime/app separation when needed

Examples:

- `IDbCap`
- `ILogCap`
- `IRuntimeCaps`
- `IAppCaps`

The public API should favor the shape:

```fsharp
type WorkflowEnv = inherit IRuntimeCaps inherit IAppCaps
```

or a concrete env type that implements those contracts.

Concrete contract examples:

```fsharp
type ILog =
    abstract Info : string -> unit

type IClock =
    abstract UtcNow : unit -> DateTimeOffset

type IDb =
    abstract Query : string -> string

type IRuntimeCaps =
    abstract Log : ILog
    abstract Clock : IClock

type IAppCaps =
    abstract Db : IDb
```

The important rule is that the contract names are the public requirement story.
Users should read signatures and immediately understand what the workflow needs.

Concrete carrier example:

```fsharp
type AppEnv =
    { Db: IDb
      Log: ILog
      Clock: IClock }
    interface IRuntimeCaps with
        member x.Log = x.Log
        member x.Clock = x.Clock
    interface IAppCaps with
        member x.Db = x.Db
```

### 3. Adapter Layer

The adapter layer is the missing link.

Responsibilities:

- project a runtime registry into a public contract
- hide the runtime storage details from user code
- remove repetitive forwarding code
- centralize tag lookup and service selection

Design rules:

- adapters should be generated when possible
- adapters should be small and obvious when handwritten
- adapters should be the only place that knows how registry entries become public contract members
- adapters should be the boundary where tagging policy is resolved

The adapter layer is what makes the library feel like a product instead of a demo.

Concrete adapter example:

```fsharp
module AppEnvAdapter =
    let fromRegistry (reg: Registry) : AppEnv =
        { Db = Registry.get<IDb> reg
          Log = Registry.get<ILog>(tag = Some "Main") reg
          Clock = Registry.get<IClock> reg }
```

If the final implementation uses generated code, the generator should emit the equivalent of the above
for every declared public contract member.

The adapter should ideally be generated from a declaration like:

```fsharp
[<CapabilityHost>]
type AppEnv =
    { Db: IDb
      Log: ILog
      Clock: IClock }
```

The generator then emits:

```fsharp
module AppEnv =
    val fromRegistry : Registry -> AppEnv
    val toRuntimeCaps : AppEnv -> IRuntimeCaps
    val toAppCaps : AppEnv -> IAppCaps
```

The adapter must be the only place that:

- knows how tags are resolved
- knows which registry key maps to which contract member
- can combine multiple named services into a single object
- can vary by host, runtime, or build target

### 4. Resource Scope

Scope is a first-class runtime primitive.

Responsibilities:

- track acquired resources
- register finalizers
- ensure release occurs exactly once
- preserve acquisition order and teardown semantics

Design rules:

- scope must be usable by layers and resource-producing services
- scope must be owned by the runtime, not by user workflow code
- scope cleanup must be deterministic
- scope semantics must work for local overrides and nested compositions

The scope is not optional. If FsFlow claims production-grade composition, lifecycle must be explicit.

Concrete lifecycle example:

```fsharp
let openDb (connectionString: string) (scope: Scope) : IDb =
    let conn = openConnection connectionString
    scope.AddFinalizer(fun () -> conn.Dispose())
    conn
```

If a service needs asynchronous acquisition, the layer should own the asynchronous boundary and only
publish the fully acquired service after the resource is ready.

Scope semantics to preserve:

- finalizers run when the outer run completes
- finalizers run even if the workflow fails
- finalizers run even if the workflow is interrupted
- finalizers should run in reverse acquisition order where possible
- finalizers must not run twice

## Public Workflow Shape

The workflow surface must stay simple.

The preferred public model is:

- `Flow<'env, 'error, 'value>` remains the core workflow type
- `'env` is usually a nominal contract or a concrete environment implementing one or more contracts
- runtime/app separation is represented explicitly when needed, not hidden

Proposed execution shape:

```fsharp
type Flow<'env, 'error, 'value> =
    'env -> CancellationToken -> Effect<'value, 'error>
```

The public builder must preserve the current `Exit` / `Cause` model, so the underlying effect type
must not collapse defects or interruptions into plain `Result`.

The runtime/app split should follow this rule:

- runtime concerns: logging, tracing, metrics, clocks, cancellation, host integration
- app concerns: repositories, domain services, feature-specific dependencies

The split may be encoded as:

- `RuntimeContext<'runtime, 'env>` or an equivalent two-part carrier
- `RuntimeCaps` and `AppCaps` interfaces
- a concrete `AppEnv` / `RuntimeEnv` type that implements the contracts

What must not happen:

- tuple-SRTP becoming the public contract story
- registry lookup surfacing directly in normal workflows
- hidden runtime semantics changing the user-visible workflow model

Example workflow against the public contract:

```fsharp
let fetchAndLog (id: int) : Flow<IAppEnv, AppError, string> =
    flow {
        let! env = Flow.env
        let data = env.Db.Query (string id)
        do! Flow.read (fun (r: IRuntimeCaps) -> r.Log.Info $"Fetched {data}")
        return data
    }
```

The exact helper names can change, but the intended shape is:

- workflows read from named contracts
- runtime and app concerns remain distinct
- the caller supplies a concrete environment implementing the needed interfaces

The preferred runtime/app carrier is:

```fsharp
type RuntimeContext<'runtime, 'env> =
    { Runtime: 'runtime
      Environment: 'env
      CancellationToken: CancellationToken }
```

When both halves are used in the same workflow, the runtime-side operations should constrain `'runtime`
and the app-side operations should constrain `'env`.

## Layering Direction

FsFlow needs a `Layer` concept, but the layer should serve the foundation rather than define it.

Layer responsibilities:

- build runtime values
- acquire resources under scope
- register services with tags
- compose upstream and downstream dependencies
- support local overrides

Layer rules:

- a layer may create or transform registry state
- a layer may produce a nominal contract adapter
- a layer must cooperate with scope cleanup
- a layer must not force users into runtime registry code

The long-term goal is to let users express:

- what they need
- what they provide
- how those providers are composed

without exposing the storage mechanics.

Concrete layer example:

```fsharp
type Layer<'input, 'error, 'output> =
    Registry -> CancellationToken -> Effect<'output, 'error>

module Layer =
    val provide : Layer<'input, 'error, 'output> -> ('output -> 'workflow) -> 'workflow
```

Layer responsibilities in practice:

```fsharp
// create a Db under scope
let dbLayer (connString: string) : Layer<Registry, AppError, IDb> =
    fun reg ct -> task {
        let conn = openConnection connString
        reg.Scope.AddFinalizer(fun () -> conn.Dispose())
        return Ok (conn :> IDb)
    }

// register a tag
let taggedLogLayer : Layer<Registry, AppError, unit> =
    fun reg ct -> task {
        let log = createLogger()
        let reg = Registry.add<ILog> ~tag:"Main" log reg
        return Ok ()
    }

// chain layers
let composed =
    dbLayer "..."
    |> Layer.bind (fun db ->
        taggedLogLayer |> Layer.map (fun () -> db))
```

The final shape can differ, but the layer must be able to:

- create services
- register them into a registry
- add finalizers into scope
- return a value or contract that later adapters can consume

## What Success Looks Like

The foundation is successful when a user can:

1. Define a few ordinary interfaces for runtime or app dependencies.
2. Define one concrete environment type that implements the required contracts.
3. Add tagged services when the same CLR type appears multiple times.
4. Build services under scope and know they will be disposed correctly.
5. Override a service for a subtree or branch without rewriting the entire composition root.
6. Write workflows whose signatures are readable and stable.
7. Use generated bridging code instead of hand-written adapter boilerplate.

Example success path:

```fsharp
let runtime =
    Runtime.build
        |> Runtime.addLog liveLog
        |> Runtime.addClock liveClock

let app =
    AppEnv.create
        |> AppEnv.addDb liveDb
        |> AppEnv.addEmail liveEmail

let result =
    Flow.run
        (RuntimeContext.create runtime app CancellationToken.None)
        workflow
```

That is the shape the docs, generators, and runtime implementation should converge on.

### End-to-End Example

This is the expected user-facing flow, end to end:

```fsharp
// 1. declare contracts
type ILog = abstract Info : string -> unit
type IClock = abstract UtcNow : unit -> DateTimeOffset
type IDb = abstract Query : string -> string

type IRuntimeCaps =
    abstract Log : ILog
    abstract Clock : IClock

type IAppCaps =
    abstract Db : IDb

// 2. declare a concrete env
type AppEnv =
    { Db: IDb
      Log: ILog
      Clock: IClock }
    interface IRuntimeCaps with
        member x.Log = x.Log
        member x.Clock = x.Clock
    interface IAppCaps with
        member x.Db = x.Db

// 3. write workflow code
let workflow (id: int) : Flow<IAppCaps, AppError, string> =
    flow {
        let! env = Flow.env
        let data = env.Db.Query (string id)
        return data
    }

// 4. build runtime storage
let reg =
    Scope()
    |> Registry.empty
    |> Registry.add<IDb> fakeDb
    |> Registry.add<ILog> ~tag:"Main" fakeLog
    |> Registry.add<IClock> fakeClock

// 5. adapt runtime storage to the contract
let env = AppEnvAdapter.fromRegistry reg

// 6. run the workflow
let result = workflow 42 env
```

The real implementation may use `Flow.run`, `TaskFlow.run`, or `RuntimeContext`, but the shape above
is the target user experience.

## What Should Not Reappear

Do not reintroduce:

- tuple-soup as the public environment shape
- SRTP-heavy public APIs
- reflection as the runtime access path
- untyped service-provider lookup in ordinary workflows
- hidden environment conventions that require source spelunking

## Remaining Direction

After the foundation lands, the next unresolved directions are still:

- preserve `Cause.Fail`, `Cause.Interrupt`, and `Cause.Die` as distinct runtime outcomes
- make defect handling explicit enough to support a ZIO-like lossless failure story
- evolve STM from a pessimistic lock model to a ZIO-like coordination engine with `retry` and `orElse`
- keep docs, examples, and generated reference aligned with the source of truth

These are still important, but they must not distract from the foundation work.

## Done Means

The foundation is not done until:

- user-facing contracts are nominal and readable
- runtime storage is internal and tagged
- scope cleanup is deterministic
- adapters are generated or mechanically derived
- runtime/app separation is explicit where needed
- `Exit` / `Cause` semantics remain intact
- docs and reference pages describe the real API rather than the design discussion

## Implementation Notes For Future Work

When implementing or revising the foundation, keep these rules explicit:

1. Prefer ordinary F# code in user-facing signatures.
2. Keep registry mechanics out of workflow examples.
3. Use code generation to remove repetitive adapter forwarding.
4. Preserve the current failure semantics instead of flattening everything into `Result`.
5. Make runtime/app separation visible in names, not just in comments.
6. Add tests for tagging, overrides, and scope cleanup before expanding layer composition.

## Acceptance Checklist For The Foundation

Use this checklist during implementation reviews:

- [ ] registry can store multiple services of the same type under different tags
- [ ] registry lookup is not exposed as normal workflow API
- [ ] scope finalizers run deterministically and exactly once
- [ ] adapters can project the same registry into multiple nominal contracts
- [ ] a concrete env can implement grouped interfaces cleanly
- [ ] runtime/app split is visible in types and examples
- [ ] local overrides can be expressed without rebuilding the entire app
- [ ] docs include a complete build/adapt/run example
- [ ] generated reference can be derived from the same source of truth
