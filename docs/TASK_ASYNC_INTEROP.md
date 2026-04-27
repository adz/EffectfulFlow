# Task And Async Interop

Task-oriented APIs described on this page belong in the `FsFlow.Net` package. The core `FsFlow`
package keeps only sync and `Async` concepts.

This page lays out the task and async boundary shapes that `flow {}` can bind directly and
the ones that should stay explicit.

This page is about boundary shapes, not business logic. The goal is to help you choose the
right shape up front and avoid confusing compiler errors later.

## The Common Shapes

These shapes already have names:

- `Task<'value>`
- `Task<Result<'value, 'error>>`
- `ValueTask<'value>`
- `ValueTask<Result<'value, 'error>>`
- `Async<'value>`
- `Async<Result<'value, 'error>>`

This shape is named by FsFlow because F# does not give it a useful built-in name:

- `ColdTask<'value>`

## Which Shapes Bind Directly

These bind directly inside `flow {}`:

- `Flow<'env, 'error, 'value>`
- `Result<'value, 'error>`
- `Async<'value>`
- `Async<Result<'value, 'error>>`
- `Task<'value>`
- `Task<Result<'value, 'error>>`
- `ColdTask<'value>`

Example:

```fsharp
let readAll path : ColdTask<string> =
    ColdTask(fun ct -> System.IO.File.ReadAllTextAsync(path, ct))

let workflow : Flow<unit, string, string> =
    flow {
        let! a = async { return "a" }
        let! b = Task.FromResult "b"
        let! c = readAll "config.json"
        return a + b + c
    }
```

## Cold Task Shapes

`ColdTask<'value>` is:

```fsharp
CancellationToken -> Task<'value>
```

Use it when work should start only when the flow runs and should observe the runtime
cancellation token.

That semantic difference matters more than the wrapper itself:

- rerunning a flow that binds `ColdTask<'value>` calls the factory again
- each run gets the current runtime `CancellationToken`
- the underlying effect can restart from scratch on each run

Example:

```fsharp
let readAll path : ColdTask<string> =
    ColdTask(fun ct -> System.IO.File.ReadAllTextAsync(path, ct))
```

When the cold boundary also returns typed failures, use:

```fsharp
ColdTask<Result<'value, 'error>>
```

Example:

```fsharp
let loadText path : ColdTask<Result<string, string>> =
    ColdTask(fun ct ->
        task {
            let! text = System.IO.File.ReadAllTextAsync(path, ct)
            return Ok text
        })
```

## When To Stay Explicit

Use the explicit helpers when you want the boundary shape to stay visible:

- `Flow.Task.fromCold`
- `Flow.Task.fromColdResult`
- `Flow.Task.fromHot`
- `Flow.Task.fromHotResult`
- `Flow.Task.fromColdUnit`
- `Flow.Task.fromHotUnit`

`ColdTask<Result<'value, 'error>>` stays explicit on purpose:

```fsharp
let workflow : Flow<unit, string, string> =
    loadText "config.json"
    |> Flow.Task.fromColdResult
```

That rule avoids ambiguity with `ColdTask<Result<'value, 'error>>`, which would otherwise
make builder overload resolution and compiler errors worse.

## Hot Task Shapes

Hot tasks are already started task values:

```fsharp
let started = Task.FromResult 42
```

Use hot task binding when you already have the task value on purpose:

```fsharp
flow {
    let! value = started
    return value
}
```

If you want the boundary to stay visible, use `Flow.Task.fromHot` or
`Flow.Task.fromHotResult`.

There is no separate `HotTask<'value>` alias because `Task<'value>` already names the shape.

Rerunning a flow that binds a hot task does not restart the effect:

- the flow re-awaits the same started task value
- the original work is not created again
- the current flow `CancellationToken` cannot be injected into that already-created task

Use hot task lifting when the task already exists and reusing that exact started work is what
you want.

## `ValueTask` Shapes

`ValueTask` follows the same hot-versus-cold split.

For an already-created `ValueTask<'value>`, convert it through `ColdTask.fromValueTask` when you
need a `ColdTask<'value>` boundary:

```fsharp
let started = ValueTask<int>(42)

let workflow : Flow<unit, string, int> =
    started
    |> ColdTask.fromValueTask
    |> Flow.Task.fromCold
```

That preserves hot semantics:

- the `ValueTask` is converted once to a started `Task`
- rerunning the flow re-awaits that same started work
- the current flow `CancellationToken` is not pushed into the original operation

That normalization step is intentional. `ValueTask` is a single-consumption oriented type, so
awaiting the same instance multiple times, calling `AsTask()` multiple times, or mixing those
consumption styles is not a safe backbone for a cold, restartable workflow.

If you need reusable hot semantics across flow reruns, normalize to `Task` or `ColdTask` before
the reusable flow is built. In practice, that means `started.AsTask()` or
`ColdTask.fromValueTask started` is the safer storage boundary than keeping a raw started
`ValueTask` around.

For cold `ValueTask` factories, prefer `ColdTask.fromValueTaskFactory` or
`ColdTask.fromValueTaskFactoryWithoutCancellation`:

```fsharp
let readAll path : ColdTask<string> =
    ColdTask.fromValueTaskFactory(fun ct ->
        ValueTask<string>(System.IO.File.ReadAllTextAsync(path, ct)))
```

That preserves cold semantics:

- rerunning the flow calls the factory again
- the factory can observe the runtime `CancellationToken`
- the effect can start again from scratch on each run

## Async Shapes

Async shapes also bind directly:

```fsharp
flow {
    let! a = async { return 42 }
    let! b = async { return Ok 1 }
    return a + b
}
```

If you want to stay explicit, use:

- `Flow.fromAsync`
- `Flow.fromAsyncResult`

There is no special alias for async because `Async<'value>` and `Async<Result<'value, 'error>>`
already name those shapes directly.

## Choosing A Shape

Use:

- `ColdTask<'value>` when you define a new task helper and need restartable execution semantics
- `ColdTask<'value>` when the operation should receive the runtime `CancellationToken`
- `ColdTask<Result<'value, 'error>>` when the cold helper also returns typed failures
- `Task<'value>` or `Task<Result<'value, 'error>>` when you already have a started task value
- `ValueTask<'value>` or `ValueTask<Result<'value, 'error>>` only as interop inputs that you adapt through `ColdTask`
- `Async<'value>` or `Async<Result<'value, 'error>>` when you are crossing an existing F# async boundary

Do not define new reusable helpers that return already-started `Task` or `ValueTask` values unless
sharing one started operation is the intended behavior.

If you are unsure, prefer `ColdTask<'value>` for task-based interop you control and use the more
explicit helper first.

## Next

Read [`docs/GETTING_STARTED.md`](./GETTING_STARTED.md) for the first examples, then
[`docs/TROUBLESHOOTING_TYPES.md`](./TROUBLESHOOTING_TYPES.md) when the compiler complains
about one of these shapes.
