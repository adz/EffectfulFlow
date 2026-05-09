---
title: Home
description: FsFlow technical guides, semantics, and API reference.
---

<div class="docs-home-hero">

<div class="docs-home-copy">

<span class="eyebrow">One model from predicate checks to task execution</span>

# A single model for Result-based programs in F#.

<p class="lede">
Write predicate checks once. Keep fail-fast logic in `Result`, accumulate sibling failures with `Validation`, then lift the same logic into `Flow`, `AsyncFlow`, or `TaskFlow` when the boundary needs environment access, async work, task interop, cancellation, or runtime policy.
![Flow](content/img/flow-graphic.png)
</p>

<div class="docs-home-meta">
<span class="docs-chip">Check -> Result -> Validation</span>
<span class="docs-chip">Flow / AsyncFlow / TaskFlow</span>
<span class="docs-chip">Typed failure</span>
<span class="docs-chip">Explicit environment</span>
<span class="docs-chip">Runtime context</span>
<span class="docs-chip">Cold execution semantics</span>
</div>

</div>
 
</div>

:::warning
API Still stabilising - wait for 1.0 to avoid breaking changes
:::

## Start Here

<div class="docs-grid">

<section class="docs-card">
<span class="label">Core Model</span>
<h2><a href="WHY_FSFLOW">The FsFlow Model</a></h2>
<p>The one mental model to keep in view: `Check`, `Result`, `Validation`, `Flow`, `AsyncFlow`, and `TaskFlow` are the same shape at different boundaries.</p>
</section>

<section class="docs-card">
<span class="label">Semantics</span>
<h2><a href="SEMANTICS">Execution Semantics</a></h2>
<p>Read this if you need the rules for cold execution, cancellation, reruns, and runtime context.</p>
</section>

<section class="docs-card">
<span class="label">Reference</span>
<h2><a href="./reference/fsflow/">API Reference</a></h2>
<p>The module index for the public surface, including `Check`, `Result`, `Validation`, `Flow`, `AsyncFlow`, `TaskFlow`, and `ColdTask`.</p>
</section>

</div>

## Next Reads

The home page should stay compact. These links cover the most common follow-ups:

- `GETTING_STARTED` for the quickest path into a real app boundary.
- `VALIDATE_AND_RESULT` for the validation-first workflow.
- `CAPS` for named capability boundaries with `Needs<'dep>` and `Env<'dep>`.
- `TASK_ASYNC_INTEROP` for `Async`, `Task`, `ValueTask`, and `ColdTask` interop.
- `examples/README` for runnable examples.
- `INTEGRATIONS` for the library map around FsFlow.
- `TROUBLESHOOTING_TYPES` for the most common compiler errors.

## Example: check once, lift later

```fsharp
open FsFlow

type RegistrationError =
    | EmailMissing
    | SaveFailed of string

let validateEmail (email: string) : Result<string, RegistrationError> =
    email
    |> Check.notBlank
    |> Check.orError EmailMissing
```

That pure `Result` can be used directly in a task-oriented application boundary:

```fsharp
open System
open System.IO
open System.Threading
open System.Threading.Tasks
open FsFlow

type User =
    { Email: string
      SettingsPath: string
      FeatureFlagsPath: string }

type RegistrationEnv =
    { Root: string
      LoadUser: int -> Task<Result<User, RegistrationError>>
      SaveUser: User -> Task<Result<unit, RegistrationError>> }

let readTextFile (path: string) : TaskFlow<RegistrationEnv, RegistrationError, string> =
    taskFlow {
        do! Check.okIf (File.Exists path)
            |> Check.orError (SaveFailed $"Missing file: {path}")

        return! ColdTask(fun ct -> File.ReadAllTextAsync(path, ct))
    }

let registerUser userId : TaskFlow<RegistrationEnv, RegistrationError, string * string> =
    taskFlow {
        let! root = TaskFlow.read _.Root
        let! loadUser = TaskFlow.read _.LoadUser
        let! saveUser = TaskFlow.read _.SaveUser

        let! user = loadUser userId
        do! validateEmail user.Email

        let settingsFile = Path.Combine(root, user.SettingsPath)
        let featureFlagsFile = Path.Combine(root, user.FeatureFlagsPath)

        let! settings = readTextFile settingsFile
        let! featureFlags = readTextFile featureFlagsFile
        do! saveUser user

        return settings, featureFlags
    }
```

`validateEmail` is just `Result<string, RegistrationError>`.
`taskFlow` lifts it directly with `do!`.
The runtime gets richer without changing how validation is expressed.

This snippet shows the core shape. The full runnable example, including `main` and temp-directory setup,
is in [`examples/FsFlow.ReadmeExample/Program.fs`](https://github.com/adz/FsFlow/blob/main/examples/FsFlow.ReadmeExample/Program.fs).

It reads `Root` and other dependencies from `'env`, reuses plain validation, and performs file reads in one `taskFlow {}`
so the cancellation token is passed implicitly into each cold task.

Run side:

```fsharp
use cts = new CancellationTokenSource()

registerUser 42
|> TaskFlow.run
    { Root = root
      LoadUser = fun _ -> Task.FromResult (Error EmailMissing)
      SaveUser = fun _ -> Task.FromResult (Ok ()) }
    cts.Token
|> Async.AwaitTask
|> Async.RunSynchronously
```
