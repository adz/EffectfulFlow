---
weight: 40
title: Managing Dependencies
description: Introduction to dependency management in FsFlow.
type: docs
---


In FsFlow, a workflow is not just a function; it is a description of work that needs an **environment** to run. 

As your application grows, you might be tempted to create a single "God Object" (like `AppEnv`) that contains every service in your system: Database, Logger, API Gateways, and more. However, if every small workflow depends on this massive record, your code becomes hard to test and tightly coupled. You'd have to mock the entire universe just to test a simple calculation.

FsFlow provides two primary architectural styles to help you "slice" your dependencies so that each workflow only asks for what it actually needs.

## The Two Styles

### 1. The Record Pattern (Environment Slicing)
This is the simplest way to start. You define your environment as a standard F# record. Workflows "read" or "project" the fields they need from this record. It is perfect for local helpers, internal logic, and smaller applications where record types are stable.

**Read the Deep Dive:** [Environment Slicing](./env-slicing/)

### 2. RuntimeContext for Host Services
When you need to split operational services from application dependencies, use `RuntimeContext<'runtime, 'env>`. This keeps logging, metrics, and similar host concerns separate from the app record.

---

## Relationship to Architecture

These two patterns support different **Architectural Styles**:

- **Style 1: The Booted App**: Usually uses a single large record (Record Pattern) for simplicity.
- **Style 2: Parameters + Context**: Uses parameters for core logic and a thin record (Record Pattern) for request context.
- **Style 3: .NET DI**: Often uses `RuntimeContext<'runtime, 'env>` and the Record Pattern to bridge .NET services into the FsFlow execution model.

For more details on these structures, see [Architectural Styles](./architectural-styles/).

---

## Comparison at a Glance

| Feature | Record Pattern | RuntimeContext |
| :--- | :--- | :--- |
| **Typical Use** | Local helpers, small apps | Host services plus app dependencies |
| **Coupling** | Bound to a specific record type | Bound to two explicit scopes |
| **Simplicity** | High (Standard F#) | Medium |
| **Flexibility** | Moderate | High |

---

## Which style should I use?

**Start with the Record Pattern.** It is the most idiomatic way to write F# and requires the least amount of boilerplate. 

Move to `RuntimeContext<'runtime, 'env>` when:
- You need to keep operational services separate from application dependencies.
- You want a thin host/service split without pushing every concern into one record.
- You are bridging a conventional `.NET` app host into FsFlow workflows.

## Shared Helpers: The Capability Module

Regardless of which style you choose, FsFlow provides a `Capability` module that works across both. These helpers let you read from the main `Flow` surface and from `RuntimeContext` without needing specific module prefixes.

```fsharp
let log message =
    flow {
        let! logger = Capability.service _.Logger
        logger.Log message
    }
```

Learn more about these polymorphic helpers in the [Environment Slicing](./env-slicing/#the-capability-module) guide.
