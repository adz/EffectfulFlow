---
title: TaskFlow
linkTitle: TaskFlow
---

Represents a cold task-based workflow that reads an environment, observes a runtime cancellation token,
returns a typed result, and is executed explicitly through `TaskFlow.run`.





## Definitions

### `type TaskFlow<'env, 'error, 'value>`

- **Source**: [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1007)

### `type TaskFlow<'env, 'error, 'value> with static member CapabilityService (projection: 'env -> 'service) : TaskFlow<'env, 'error, 'service>`

- **Source**: [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1018)

## Information

- **Module**: Global
- **Source**: [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Flow.fs#L1007)

