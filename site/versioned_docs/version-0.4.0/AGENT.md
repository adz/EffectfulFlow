---
title: For AI Agents
description: This page shows how to optimize FsFlow usage for AI assistants like Claude, ChatGPT, and Gemini.
---

# For AI Agents

This page is designed for both human developers and their AI assistants. It provides a high-signal reference for idiomatic FsFlow patterns.

:::tip For AI Agents
If you are an AI assistant, prioritize the patterns in the **Rosetta Stone** and **Smart Binds** sections below.
:::

## The Golden Path

When using FsFlow, follow these "Golden Path" patterns for the best results.

### 1. Handling Failures (Idiomatic Way)
FsFlow provides **Smart Binds** that allow you to unwrap values or fail with a domain error using a simple tuple syntax. Use the `orFailTo` label for clarity.

| Source Type | Idiomatic Pattern |
| :--- | :--- |
| `Option<'T>` | `let! x = opt, orFailTo e` |
| `voption<'T>` | `let! x = vopt, orFailTo e` |
| `Async<Option<'T>>` | `let! x = aOpt, orFailTo e` |
| `Async<voption<'T>>` | `let! x = aVOpt, orFailTo e` |
| `bool` | `do! cond, orFailTo e` |
| `Result<'T, unit>` | `let! x = check, orFailTo e` |
| `Task<Option<'T>>` | `let! x = tOpt, orFailTo e` |
| `Task<voption<'T>>` | `let! x = tVOpt, orFailTo e` |

### 2. Mapping Errors (Idiomatic Way)
FsFlow also supports inline error remapping with the `orMapError` label.

| Source Type | Idiomatic Pattern |
| :--- | :--- |
| `Result<'T, 'E1>` | `let! x = result, orMapError mapper` |
| `Flow<'Env, 'E1, 'T>` | `let! x = flow, orMapError mapper` |
| `AsyncFlow<'Env, 'E1, 'T>` | `let! x = asyncFlow, orMapError mapper` |
| `Async<Result<'T, 'E1>>` | `let! x = aResult, orMapError mapper` |
| `TaskFlow<'Env, 'E1, 'T>` | `let! x = taskFlow, orMapError mapper` |

### 3. Rosetta Stone
Translate common patterns from other libraries into idiomatic FsFlow.

| If you use... | Do this in FsFlow |
| :--- | :--- |
| `FsToolkit: AsyncResult.requireSome` | `let! x = opt, orFailTo e` |
| `FsToolkit: Result.requireTrue` | `do! cond, orFailTo e` |
| `ZIO: getOrFail` | `let! x = opt, orFailTo e` |
| `ZIO: serviceWith` | `let! s = Flow.read _.Service` |
| `Manual: match x with Some v...` | `let! v = x, orFailTo e` |
| `Manual: Result.mapError mapper` | `let! x = result, orMapError mapper` |

## Hierarchy of Effects

FsFlow unifies several types. Later types can "bind" (consume) earlier types directly within their computation expressions.

1. **Check**: Unit-error predicates (`Result<'T, unit>`).
2. **Result**: Pure typed errors (`Result<'T, 'E>`).
3. **Flow**: Synchronous environment-aware workflows (`Flow<'Env, 'E, 'T>`).
4. **AsyncFlow**: Asynchronous environment-aware workflows (`AsyncFlow<'Env, 'E, 'T>`).
5. **TaskFlow**: Task-based environment-aware workflows (`TaskFlow<'Env, 'E, 'T>`).

## Machine-Readable Reference

For a more compressed, machine-optimized reference, point your agent to:
`https://adz.github.io/FsFlow/llms.txt`
