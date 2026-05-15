# FsFlow Documentation and Implementation Plan

## Overview

The FsFlow codebase contains a dual architecture for capabilities: a rigid, record-based "Current" runtime and a flexible, dictionary-based "Future" registry. The documentation is currently lagging behind both, promoting patterns that are either internally deprecated or over-complicated.

This plan outlines the steps to align the implementation with the "Capability Family" vision and update the documentation to match.

## 1. Implementation Gaps & Misalignments

### A. The Fixed Runtime Record
**Status:** `RuntimeContext` in `Capability.fs` is an internal, fixed record of 5 services.
**Issue:** This prevents modular capability packages (FileSystem, Http, etc.) from being truly "ambient" without editing the core library.
**Plan:** 
- Transition `RuntimeContext` from a record to the `Registry` model.
- Surface a public `Runtime` type that wraps the internal `Registry`.

### B. Two-Parameter Flow vs. Three-Parameter Intent
**Status:** `Flow<'env, 'error, 'value>` is the only public type.
**Issue:** Research documents suggest `RuntimeContext<'runtime, 'env>` or similar to separate operational vs. domain concerns.
**Plan:**
- Keep `Flow` as two-parameter for simplicity.
- Explicitly document that "Ambient" capabilities are part of the hidden `Runtime` state (via `AsyncLocal`), while `'env` is for user-domain dependencies.
- Introduce `Flow.readRuntime` to access the registry-backed services.

### C. Duplicate Capability Definitions
**Status:** `IClock`, `ILog`, etc., are defined in `src/FsFlow/Capability.fs` and then aliased/redefined in `src/FsFlow.Capabilities.Core/Core.fs`.
**Issue:** Maintenance burden and confusion about source of truth.
**Plan:**
- Move all core capability interfaces to a shared `FsFlow.Core` namespace or consolidate them in the `Core` capability package.

### D. The Layer Story
**Status:** `Layer.provideLayer` (public) is just environment mapping. `RuntimeLayer` (internal) handles scopes and registries.
**Issue:** The public API doesn't support the "resource acquisition" or "registry modification" features of the internal machinery.
**Plan:**
- Evolve the public `Layer` API to support the `Registry` model, allowing users to build a runtime from a DI container or configuration.

## 2. Documentation Plan

### Phase 1: Foundations (The "Levels" Story)
- **Update `managing-dependencies/_index.md`:** Introduce the "Levels of Capabilities" (Explicit Records -> Runtime -> IServiceProvider -> Nominal Helpers).
- **Create `core-model/runtime.md`:** Explain the "Ambient Runtime" (Clock, Logging, etc.) and how it differs from the "User Environment" (`env`).
- **Surface `Flow.withClock` / `Flow.withLog`:** Move these from "Ambient Black Box" to "Explicit Runtime Overrides."

### Phase 2: Capability Families
- **Pivot `tutorials/capabilities.md`:** Stop teaching `IHasX` for domain repositories. Instead, teach how to use `Clock.now` and `Log.info` as "System Capabilities."
- **Update `reference/capability/`:**
    - Deprecate `Requires` and `Resolve` tokens.
    - Document `Runtime.read` and `Runtime.provide`.
- **Add FileSystem and Console examples:** Show how modular packages extend the "Ambient Runtime."

### Phase 3: The Host Edge
- **Update `managing-dependencies/provider-edge.md`:** Show how to use `FsFlow.Hosting` to build a `Runtime` from `IServiceProvider`.
- **Refactor `tutorials/app-host.md`:** Use the `Registry` model to provision services, showing a clean separation between "Operational Services" (from DI) and "Domain Services" (in a record).

## 3. Specific Documentation Tasks

| Topic | Current State | Target State |
| :--- | :--- | :--- |
| **Capability Identity** | Confusion between interfaces and packages. | "Capability" means a modular effect family (e.g., FileSystem). |
| **Ambient Runtime** | Internal record, poorly explained. | Registry-backed, modularly extensible. |
| **Domain Deps** | Promotes `IHasX` (Boilerplate-heavy). | Promotes Plain Records (Simple & Agent-friendly). |
| **Layers** | Simple environment mapping. | Provisioning/Registry composition unit. |
| **Reference** | Noisy with stale binding tokens. | Focused on the "Cap Family" operations. |

## 4. Immediate Next Steps for Developers

1.  **Refactor `RuntimeContext` to use `Registry`** in `src/FsFlow/Capability.fs`.
2.  **Make `Runtime` a public concept** (even if opaque).
3.  **Update `FsFlow.Capabilities.Core`** to use the new `Registry` lookup.
4.  **Delete/Demote `Requires` and `Resolve`** to `internal` if possible, or mark as `[<Obsolete>]`.
