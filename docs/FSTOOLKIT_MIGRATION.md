# Migration Guide

Read this page when you already have `Async<Result<_,_>>` or FsToolkit-style workflows and want to adopt the FsFlow workflow family incrementally without rewriting the whole application.

The migration path is: keep the code that is already honest, choose the workflow family that matches the runtime you already have, then move one use case at a time.

## Start From The Existing Shape

A typical pre-FsFlow use case already looks like this:

```fsharp
type AppEnv =
    { LoadUser: int -> Async<Result<string, string>>
      Prefix: string }

let greet userId (env: AppEnv) : Async<Result<string, string>> =
    async {
        let! loaded = env.LoadUser userId

        match loaded with
        | Error error ->
            return Error error
        | Ok name ->
            return Ok $"{env.Prefix} {name}"
    }
```

Do not rewrite pure validation or domain logic first.
Move the workflow boundary first.

## Step 1. Keep Existing `Async<Result<_,_>>` Helpers Honest

If the existing helper already returns `Async<Result<_,_>>`, `AsyncFlow` is usually the smallest useful migration:

```fsharp
let greet userId : AsyncFlow<AppEnv, string, string> =
    asyncFlow {
        let! loadUser = AsyncFlow.read _.LoadUser
        let! name = loadUser userId
        let! prefix = AsyncFlow.read _.Prefix
        return $"{prefix} {name}"
    }
```

This avoids inventing a task-oriented wrapper where the code is already `Async`.

## Step 2. Keep Validation Plain

Do not convert plain `Result` helpers just because the outer workflow moved.

```fsharp
let validateName name =
    if System.String.IsNullOrWhiteSpace name then
        Error "missing name"
    else
        Ok name

let greet userId : AsyncFlow<AppEnv, string, string> =
    asyncFlow {
        let! loadUser = AsyncFlow.read _.LoadUser
        let! loadedName = loadUser userId
        let! validName = validateName loadedName
        let! prefix = AsyncFlow.read _.Prefix
        return $"{prefix} {validName}"
    }
```

Keep the pure parts pure.

## Step 3. Switch To `TaskFlow` Only When The Workflow Is Really Task-Oriented

When a dependency is truly `.NET Task`-based, use `TaskFlow` instead of forcing task work through a smaller family:

```fsharp
type AppEnv =
    { LoadUser: int -> Task<Result<string, string>>
      Prefix: string }

let greet userId : TaskFlow<AppEnv, string, string> =
    taskFlow {
        let! loadUser = TaskFlow.read _.LoadUser
        let! loadedName = loadUser userId
        let! prefix = TaskFlow.read _.Prefix
        return $"{prefix} {loadedName}"
    }
```

Use `TaskFlow` when the workflow should run as a task and observe a runtime `CancellationToken`.

## Step 4. Keep Execution At The App Edge

The old style:

```fsharp
greet 42 env |> Async.RunSynchronously
```

The migrated style for `AsyncFlow`:

```fsharp
greet 42
|> AsyncFlow.toAsync env
|> Async.RunSynchronously
```

The migrated style for `TaskFlow`:

```fsharp
greet 42
|> TaskFlow.toTask env cancellationToken
|> Async.AwaitTask
|> Async.RunSynchronously
```

That is the main runtime change: execution becomes explicit through the workflow family rather than being hidden in the helper shape.

## Step 5. Migrate One Use Case At A Time

You do not need a flag day migration.

- Keep existing `Async<Result<_,_>>` helpers unchanged until a use case gets clearer as `AsyncFlow`
- Keep pure `Result` helpers unchanged
- Use `TaskFlow` only where `.NET Task` is the honest workflow boundary
- Switch environment passing only when the workflow genuinely benefits from `read`, `env`, or `localEnv`

If you already use FsToolkit computation expressions, the important question is not which CE you used before.
The important question is whether this particular use case gets clearer once dependencies, typed errors, and the real runtime shape live in one place.

## Next

Read [`docs/GETTING_STARTED.md`](./GETTING_STARTED.md) for the workflow-family overview,
[`docs/TASK_ASYNC_INTEROP.md`](./TASK_ASYNC_INTEROP.md) for the direct binding surface,
and [`docs/TINY_EXAMPLES.md`](./TINY_EXAMPLES.md) for the smallest complete examples.
