---
weight: 25
title: The Guard
type: docs
description: Bridging pure checks and effectful sources into Flow.
---

# The Guard

The [**Guard**]({{< relref "/reference/flow/t-flow.md" >}}) turns source shapes like `Option`,
`ValueOption`, `bool`, `Result<'value, unit>`, `Validation<'value, unit>`, and the `Async` / `Task`
 / `ValueTask` versions of those into bindable results.

Inside a `flow {}`, bind the effect first, then apply `Guard` to the value the effect returned.
That keeps the source shape explicit and keeps the domain error at the binding site.

## What Guard Works On

`Guard.Of` works on:

- `Option<'value>`
- `ValueOption<'value>`
- `bool`
- `Result<'value, unit>`
- `Validation<'value, unit>`
- `Async<Option<'value>>`, `Task<Option<'value>>`, `ValueTask<Option<'value>>`
- `Async<ValueOption<'value>>`, `Task<ValueOption<'value>>`, `ValueTask<ValueOption<'value>>`
- `Async<bool>`, `Task<bool>`, `ValueTask<bool>`
- `Async<Result<'value, unit>>`, `Task<Result<'value, unit>>`, `ValueTask<Result<'value, unit>>`

`Guard.MapError` works on:

- `Result<'value, 'error>`
- `Validation<'value, 'error>`
- `Flow<'env, 'error, 'value>`
- `Async<Result<'value, 'error>>`
- `Task<Result<'value, 'error>>`
- `ValueTask<Result<'value, 'error>>`

## Why It Exists

Consider a simple user lookup. When it is pure, `Check` works directly:

```fsharp
open FsFlow
open FsFlow.Check

type User = { Name: string }
type AppError = UserNotFound | InvalidPassword

// A pure "database" lookup
let tryGetUser name : User option =
    let users = [ { Name = "ada" }; { Name = "bob" } ]
    users |> List.tryFind (fun u -> u.Name = name)

let loginPure name =
    result {
        let! user = tryGetUser name |> okIfSome |> orError UserNotFound
        return user
    }
```

However, in a real application, lookup usually requires a database connection from the environment or performs async work. Then it becomes a [`Flow`]({{< relref "/reference/flow/t-flow.md" >}}):

```fsharp
// In reality, this needs the environment and is effectful
let tryGetUserFlow name : Flow<DbEnv, AppError, User option> =
    flow {
        let! db = Flow.read _.Db
        return! db.Users.TryFind name
    }
```

If you try to apply `Check` directly to the `Flow`, it fails to compile.
Bind the `Flow` first, then guard the value it returns:

```fsharp
let login name =
    flow {
        let! maybeUser = tryGetUserFlow name
        let! user = maybeUser |> Guard.Of UserNotFound
        return user
    }
```

`Guard` works on the value, not on the `Flow` itself. `flow {}` binds the effectful source; `Guard`
maps the resulting `Option`, `Check`, `Result`, or `Validation` into the domain error you choose.

```fsharp
let login name password =
    flow {
        let! maybeUser = tryGetUserFlow name
        let! user = maybeUser |> Guard.Of UserNotFound
        do! notBlank password |> Guard.Of InvalidPassword
        return user
    }
```

## Common Guard Patterns

### Guarding Options
Convert a `Some` value into success and `None` into a specific error:
```fsharp
let! user = maybeUser |> Guard.Of UserNotFound
```

### Guarding Checks
Apply a pure predicate and fail the flow if it returns `Error ()`:
```fsharp
do! notBlank password |> Guard.Of InvalidPassword
```

### Remapping Errors
If a source already has an error, but it doesn't match your flow's error type, use `Guard.MapError`:
```fsharp
let! user = fetchUser id |> Guard.MapError (fun ex -> DatabaseError ex.Message)
```

## Summary

Use **Guard** whenever you need to:
- Assign a domain error to a pure `Check` or `Option` inside a computation.
- Bind an effectful source first, then turn its `Option`, `Check`, `Result`, or `Validation` into a domain error.
- Keep your business logic readable by avoiding manual matching or re-lifting inside your flows.
