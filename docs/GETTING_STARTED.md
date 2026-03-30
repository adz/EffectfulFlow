# Getting Started

This guide is for the first hour with Effect.FS.

The goal is simple:

- keep pure logic as plain F#
- introduce `Effect` only where dependencies, async work, tasks, or typed failures begin
- make the workflow read like the happy path

## 1. The Core Type

The center of the library is:

```fsharp
Effect<'env, 'error, 'value>
```

Read that as:

- `'env`: the dependencies or context required to run
- `'error`: the expected typed failures
- `'value`: the success value

## 2. Start With Pure Validation

Keep pure checks as ordinary `Result` functions:

```fsharp
type ValidationError =
    | MissingName

let validateName (name: string) =
    if System.String.IsNullOrWhiteSpace name then
        Error MissingName
    else
        Ok name
```

Do not put everything into `Effect` immediately. The library works best when pure code stays pure.

## 3. Introduce An Effect Workflow At The Boundary

When you need dependencies or async work, switch to `effect {}`:

```fsharp
type AppEnv =
    { Prefix: string }

type AppError =
    | ValidationFailed of ValidationError

let greet : string -> Effect<AppEnv, AppError, string> =
    fun input ->
        effect {
            let! env = Effect.environment
            let! name = validateName input |> Result.mapError ValidationFailed
            return $"{env.Prefix} {name}"
        }
```

## 4. Run The Effect Explicitly

Effects are cold. You execute them explicitly:

```fsharp
let result =
    greet "Ada"
    |> Effect.execute { Prefix = "Hello" }
    |> Async.RunSynchronously
```

That produces:

```fsharp
Ok "Hello Ada"
```

## 5. Interop With Async And Task

Inside `effect {}` you can bind directly from the common wrapper types:

```fsharp
effect {
    let! value1 = Ok 1
    let! value2 = async { return 2 }
    let! value3 = System.Threading.Tasks.Task.FromResult 3
    return value1 + value2 + value3
}
```

You do not have to manually wrap each of those with helper calls first.

## 6. Access Dependencies Explicitly

For simple access, use:

- `Effect.environment`
- `Effect.read`
- `Effect.withEnvironment`
- `Effect.provide`

Example:

```fsharp
let getPrefixLength : Effect<AppEnv, AppError, int> =
    effect {
        let! env = Effect.environment
        return env.Prefix.Length
    }
```

## 7. Why `Effect.environment` Sometimes Shows Type Arguments

You asked about this shape:

```fsharp
let! env = Effect.environment<AppEnv, AppError>
```

That is sometimes needed, but not always.

Most of the time, if the surrounding workflow type is already known, this shorter form works:

```fsharp
let! env = Effect.environment
```

F# only needs the explicit generic arguments when type inference does not yet know:

- what environment type the workflow should use
- what error type the workflow should use

So the longer form is usually an inference fallback, not the preferred style.

## 8. If You Prefer A Lambda-Style Environment

If the environment is used throughout a workflow, this can read well:

```fsharp
let greet : Effect<AppEnv, AppError, string> =
    Effect.environmentWith(fun env ->
        effect {
            let! name = validateName env.Input |> Result.mapError ValidationFailed
            return $"{env.Prefix} {name}"
        })
```

That gives you the environment once as a lambda parameter instead of repeating:

```fsharp
let! env = Effect.environment
```

This is not the same as making `env` a native computation-expression parameter. It is just a convenience helper built on top of the existing effect model.

## 9. Add Operational Helpers Where They Help

The core practical helpers are:

- `Effect.retry`
- `Effect.timeout`
- `Effect.log`
- `Effect.logWith`
- `Effect.bracket`
- `Effect.bracketAsync`
- `Effect.usingAsync`

Use them where they make the workflow clearer. Do not add them everywhere by default.

## 10. Coming From FsToolkit

If you already have `Async<Result<_,_>>` code, you can migrate incrementally:

- `Effect.fromAsyncResult`
- `Effect.toAsyncResult`
- `AsyncResultCompat`

That lets you move one workflow at a time rather than rewriting a whole codebase.

## 11. Recommended Reading Order

After this guide:

1. [`examples/EffectFs.Examples/Program.fs`](../examples/EffectFs.Examples/Program.fs)
2. [`docs/FSTOOLKIT_MIGRATION.md`](./FSTOOLKIT_MIGRATION.md)
3. [`src/EffectFs/Effect.fs`](../src/EffectFs/Effect.fs)
