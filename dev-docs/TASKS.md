# FsFlow Tasks

This file is the active queue for `scripts/ralph-loop-tasks.sh`.
Keep completed work out of this file.
Keep settled design decisions in `dev-docs/decisions/`.
Keep live product and architecture direction in `dev-docs/PLAN.md`.

1. [ ] Make `Cause.Die` a first-class, discoverable defect branch.
   - Add a public `Flow.die` constructor that creates `Exit.Failure (Cause.Die exn)`.
   - Keep the low-level helper available only as implementation plumbing if needed.
2. [ ] Capture unexpected exceptions as defects at the runtime boundary.
   - Align `Flow.run`, `AsyncFlow.run`, and `TaskFlow.run` so uncaught non-cancellation exceptions become `Cause.Die`.
   - Preserve `Cause.Interrupt` for cancellation and `Cause.Fail` for expected domain errors.
3. [ ] Preserve defects through combinators and adapters.
   - Make `mapError`, `fold`, `zip`, `orElse`, retry, and interop paths keep `Cause.Die` intact.
   - Ensure defect-aware helpers do not accidentally downgrade bugs into typed errors.
4. [ ] Update user-facing docs after the runtime behavior lands.
   - Keep `docs/core-model/semantics.md`, `docs/core-model/defects.md`, and `docs/start/getting-started.md` aligned with the final runtime behavior.
   - Update retry and interruption guidance so it explains which causes are retried, rethrown, or translated.
