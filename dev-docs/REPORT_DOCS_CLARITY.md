# FsFlow Documentation Clarity Report

## Overview

This report evaluates the clarity and consistency of the FsFlow documentation, specifically regarding the "Capability" story, based on a review of `docs/` and `dev-docs/`.

## Key Findings

### 1. The "Capability" Identity Crisis
There is a significant disconnect between the **current public documentation** and the **internal design direction** found in `dev-docs/`.

*   **Public Docs:** Heavily promote the "Nominal Capability" pattern (`IHasX` interfaces) for domain dependencies. This is presented as the "standard" way to manage dependencies.
*   **Internal Research (`NEW-APPROACH.md`):** Explicitly states that domain dependencies should be "boring" (plain records or `IServiceProvider`) and that forcing everything into `IHasX` is "unsatisfying" and "expensive argument passing."
*   **Conflict:** A user following the `Tutorial: Capabilities` will implement exactly what the lead designers are now trying to move away from.

### 2. Missing Core Architecture: `RuntimeContext`
The `NEW-APPROACH.md` and `CAPS_RECOMMENDED_MODEL.md` revolve around `RuntimeContext<'runtime, 'env>` (or a three-parameter variant) to separate operational concerns (Clock, Logging) from domain concerns.

*   **Public Docs:** `RuntimeContext` is **completely absent**. Users only see `Flow<'env, 'error, 'value>`, leading to confusion about where "Ambient" capabilities (like `Clock.now`) actually live.
*   **Clarity Issue:** The term "Ambient Runtime" is used in `caps-core` reference pages, but without `RuntimeContext` documented, it's unclear how these "ambient" services are provisioned or how they relate to the `env` passed to `Flow.run`.

### 3. Stale Reference Material
The `docs/reference/capability/` section contains "Binding tokens" like `Requires`, `Resolve`, and `Resolver`.

*   **Issue:** These feel like leftovers from the rejected "Structural Accessors" or "Binding Token" research phase. They add noise and complexity to the reference surface without a clear modern use case in the "Cap Family" vision.

### 4. Inconsistent Terminology
*   **"Capability":** Is it an interface (`IHasOrders`)? Is it a package (`FsFlow.Capabilities.Core`)? Is it an operation (`Clock.now`)? The docs use it for all three interchangeably.
*   **"Environment" vs "Runtime":** The docs often conflate the two. If I use `Clock.now`, is it reading from `env`? Or from a hidden "runtime" slot?

### 5. The Layer Story
`Layer.provideLayer` is documented as a reference, but its role as the "provisioning/adaptation" mechanism (as described in `NEW-APPROACH.md`) is not clearly explained to the user. Users are left wondering when to use a "Layer" vs just creating a record.

## Verdict

The documentation is **clear about how the old model worked** (Record-based `env`) but **highly confusing and inconsistent about the new model** (Capability Families + RuntimeContext).

The library's strongest vision—"Capifying the ambient .NET/System effects"—is partially visible in `caps-core`, but it is buried under tutorials that still preach the "everything is an interface" approach which the team has internally rejected as too boilerplate-heavy.

## Recommendations for Immediate Clarity

1.  **Define the Levels:** Adopt the "Levels of Capabilities" (Explicit Records -> RuntimeContext -> IServiceProvider -> Nominal Helpers) in the public `managing-dependencies` index.
2.  **Surface `RuntimeContext`:** Move `RuntimeContext` from research to core documentation. It is the missing link for understanding how "Ambient" capabilities work.
3.  **Pivot the Tutorial:** Refactor the `Capabilities` tutorial to focus on *system* effects (Clock, FileSystem) rather than domain repositories.
4.  **Deprecate Binding Tokens:** Mark `Requires` and `Resolve` as internal or advanced/legacy to reduce the mental surface area for new users.
5.  **Clarify "Ambient":** Explicitly document how `Flow.run` (or a future `Runtime.run`) maps implementations to the slots used by `Clock.now`, `Log.info`, etc.
