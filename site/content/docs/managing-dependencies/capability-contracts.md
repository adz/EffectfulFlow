---
weight: 50
title: "Nominal Capability Contracts"
description: Small named interfaces for reusable helpers and public capability boundaries.
type: docs
---


In FsFlow, a capability is a named interface that describes an application dependency in `env`.
A capability contract puts that interface in the environment surface type.

Using an interface through a capability contract makes the dependency visible in the type, so the
compiler can check it, refactoring can move safely, and reusable helpers can advertise what they
need.

This page is for readers who want the app dependency surface to be part of the type:

- the record is getting too wide
- the dependency list is becoming boilerplate
- you want the compiler to notice when a refactor changes what an effect needs
- you want the effect signature to read like a dependency statement, not a plumbing report

The contract should be small, named, and stable. When it grows, split the bundle into smaller app
contracts or move the boundary back to a concrete record.

## The Contract

```fsharp
type IOrderRepository =
    abstract Save : Order -> unit

type IEmailSender =
    abstract SendConfirmation : Order -> unit

type IOrderCaps =
    abstract Orders : IOrderRepository
    abstract Email : IEmailSender
```

The contract is the application dependency surface, not the lookup mechanism.

Runtime-owned services such as clock and logging are implicit in the flow runtime. They are not
part of `env`, and they can be overridden with `Flow.withClock` / `Flow.withLog` when you need a
deterministic test or a scoped local change.

## Reading Through The Contract

Use the contract directly from workflows and helper functions.

```fsharp
let saveAndEmail order : Flow<#IOrderCaps, AppError, unit> =
    flow {
        let! orders = Flow.read _.Orders
        let! email = Flow.read _.Email
        orders.Save order
        email.SendConfirmation order
    }
```

In practice, the contract often comes from a concrete record or adapter type that implements the
interface.

## Why Use A Contract

Capability contracts give you:

- compiler checks on the app dependency surface
- refactor safety when a helper changes what it needs
- workflow signatures that read as “this effect needs logging, clock, and db”
- reusable helpers that advertise what they depend on
- programming to interfaces, but for effects

That makes app effects visible in the flow without forcing every boundary to become a big record.
If the boundary is already obvious, a record already gives you direct access. A contract adds a
type-level name that the compiler checks and that helpers can reuse across flows.

## What A Record Gives You

A record gives you:

- a short dependency list with direct fields
- a boundary that stays local to one area
- a shape that is easy to reuse as data
- no extra interface for a structure that is already simple

## What A Contract Gives You

A contract gives you:

- the same dependency shape named once and reused across flows or helpers
- compiler errors at the call site when a dependency changes
- a flow that advertises its capabilities directly
- a readable type instead of a long parameter bag

## Binding Tokens

`Requires<'dep>`, `Resolve<'dep>`, and `Resolve<'dep, 'value>` are compatibility binding tokens
for single-dependency projections.

Use `Flow.read` and small app contracts first. Reach for these tokens only when a single dependency
request is clearer than naming a broader app contract.

## What A Named Contract Gives You

A named contract gives you:

- a helper shared across multiple areas
- a stable dependency shape with one name
- a signature that explains itself
- a reusable interface that justifies the extra type-level name

## When It Is Not Worth It

Do not add a named contract just to make the code look more abstract.

A record keeps a concrete boundary direct.
Runtime overrides separate operational services from the app environment, and you can override
them in tests without changing the app contract.
Standard `.NET` AppHost plus DI adapts the container once at the host edge.

## What This Replaces

The current shape is simple:

- a concrete record boundary
- a small nominal interface
- `Flow.read` for app dependencies
- `Flow.withClock` / `Flow.withLog` / `Flow.withRandom` / `Flow.withGuid` for runtime-owned services

See the [Capability reference](../../reference/capability/) for the compatibility binding tokens,
edge helpers, and layer helper.
