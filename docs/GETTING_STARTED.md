# Getting Started

Read this page when you want to write your first small Effect.FS workflow.

The goal is to keep pure code as plain F#, then introduce `Effect` only at the boundary where dependencies, async work, or typed failures start.

## 1. Start With Pure Code

Keep pure validation as ordinary `Result` functions:

```fsharp
type ValidationError =
    | MissingName

let validateName (name: string) =
    if System.String.IsNullOrWhiteSpace name then
        Error MissingName
    else
        Ok name
```

You do not need `Effect` for code that is already clear as plain `Result`.

## 2. Introduce `Effect` Without An Environment First

Start with the smallest useful effect:

```fsharp
Effect<'env, 'error, 'value>
```

Example:

```fsharp
let greet input : Effect<unit, ValidationError, string> =
    effect {
        let! name = validateName input
        return $"Hello {name}"
    }
```

Read the type as:

- `'env`: the environment the workflow needs
- `'error`: the expected typed failure
- `'value`: the success value

## 3. Run The Effect Explicitly

Effects are cold. They do nothing until you execute them:

```fsharp
let result =
    greet "Ada"
    |> Effect.execute ()
    |> Async.RunSynchronously
```

Result:

```fsharp
Ok "Hello Ada"
```

Use `unit` for `'env` when the workflow does not need any dependencies yet.

## 4. Map The Whole Effect

Once you have an effect, transform the success value with `Effect.map`:

```fsharp
let greetingLength =
    greet "Ada"
    |> Effect.map String.length

let result =
    greetingLength
    |> Effect.execute ()
    |> Async.RunSynchronously
```

Result:

```fsharp
Ok 9
```

This keeps the workflow focused on producing the main value, then applies the next step to the whole effect.

## 5. Do The Same Inline

You can also build the workflow inline, then map it:

```fsharp
let result =
    effect {
        let! name = validateName "Ada"
        return $"Hello {name}"
    }
    |> Effect.map String.length
    |> Effect.execute ()
    |> Async.RunSynchronously
```

This is a good shape when the workflow is short and only used once.

## 6. Bind Existing Wrapper Types Directly

Inside `effect {}` you can bind directly from common F# and .NET shapes:

```fsharp
let workflow : Effect<unit, string, int> =
    effect {
        let! a = Ok 1
        let! b = async { return 2 }
        let! c = System.Threading.Tasks.Task.FromResult 3
        return a + b + c
    }
```

This is the main ergonomic goal of the library: keep the workflow close to the happy path even when the inputs come in different wrapper shapes.

## 7. Add The Environment When You Need Dependencies

Use the environment helpers when the workflow depends on external context:

- `Effect.environment` to read the whole environment
- `Effect.read` to project one value from it
- `Effect.withEnvironment` to run a smaller effect inside a larger environment
- `Effect.provide` to pre-supply the environment

Example:

```fsharp
type AppEnv =
    { Prefix: string }

let greetWithPrefix input : Effect<AppEnv, ValidationError, string> =
    effect {
        let! env = Effect.environment
        let! name = validateName input
        return $"{env.Prefix} {name}"
    }

let prefixLength : Effect<AppEnv, ValidationError, int> =
    Effect.read (fun env -> env.Prefix.Length)
```

## 8. Prefer The Inferred `environment` Form

Most of the time you can write:

```fsharp
let! env = Effect.environment
```

The longer form:

```fsharp
let! env = Effect.environment<AppEnv, ValidationError>
```

is only needed when F# cannot infer the environment and error types yet.

## 9. Use `environmentWith` When It Reads Better

If the workflow uses the environment throughout, this can be a cleaner shape:

```fsharp
let greet : Effect<AppEnv, ValidationError, string> =
    Effect.environmentWith(fun env ->
        effect {
            return $"{env.Prefix} world"
        })
```

Use it when it reduces repetition. Do not force it into every workflow.

## 10. Add Operational Helpers Only Where They Help

Effect.FS includes helpers for common application concerns:

- `Effect.retry`
- `Effect.timeout`
- `Effect.log`
- `Effect.logWith`
- `Effect.bracket`
- `Effect.bracketAsync`
- `Effect.usingAsync`

Add them when they make the workflow clearer. Keep the core flow small and direct.

## 11. Migrate One Workflow At A Time

If you already have `Async<Result<_,_>>` code, start at the boundary:

- lift old code with `Effect.fromAsyncResult`
- run an effect as `Async<Result<_,_>>` with `Effect.toAsyncResult`
- use `AsyncResultCompat` during migration if that keeps the transition simpler

## Next

Read [`examples/README.md`](../examples/README.md) to see complete workflows, then [`docs/FSTOOLKIT_MIGRATION.md`](./FSTOOLKIT_MIGRATION.md) if you are moving from FsToolkit.
