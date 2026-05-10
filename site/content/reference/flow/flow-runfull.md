---
title: Flow.runFull
linkTitle: runFull
type: docs
---

Executes a flow with an explicit cancellation token.


```fsharp
let runFull (environment: 'env) (cancellationToken: CancellationToken) (flow: Flow<'env, 'error, 'value>)
```




## Information

- **Module**: `Flow`
- **Source**: [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L17)

