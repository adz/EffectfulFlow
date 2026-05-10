---
title: Flow.run
linkTitle: run
---

Executes a flow with the provided environment and the default cancellation token.


```fsharp
let run (environment: 'env) (flow: Flow<'env, 'error, 'value>)
```




## Information

- **Module**: `Flow`
- **Source**: [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L35)

## Examples

```fsharp
let flow = Flow.read (fun env -> $"Hello, {env}!")
let result = Flow.run "World" flow
// result = Promise that resolves to Ok "Hello, World!" on Fable, or Ok "Hello, World!" on .NET
```

