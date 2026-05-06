# FsFlow Tasks

This file is the active queue for `scripts/ralph-loop-tasks.sh`.
Keep completed work out of this file.
Keep settled design decisions in `dev-docs/decisions/`.
Keep live product and architecture direction in `dev-docs/PLAN.md`.

The numbered items below are intentionally linear so the ralph loop can move through them one at a time.

1. [ ] Update the user-facing docs to introduce `Guard` as a central concept that evolves from `Check` / validation, including the getting started guide, semantics guide, validation/result guide, task/async interop guide, index, why-fsflow, examples, llms.txt, and any versioned docs that mirror those pages.
2. [ ] Regenerate and verify the API reference docs so the new `Guard` surface, updated `Check` surface, and any removed tuple smart-bind references are reflected consistently in `docs/reference/fsflow` and the versioned site output.
3. [ ] Normalize the alternate-computation APIs across all flow families so `orElse` / `orElseWith` consistently mean fallback to another result/validation/flow of the same family, returning the family type rather than raw inner results.
4. [ ] Replace `Check`'s error-bridging naming with `Check.orError`, removing the old `Check.orElse` / `Check.orElseWith` bridge semantics from `Check` while keeping the predicate algebra intact.
5. [ ] Sweep examples, tests, and docs for the new `Guard` / `Check.orError` / `orElse` naming so all code samples, matrices, and cross-references match the final API shape.
