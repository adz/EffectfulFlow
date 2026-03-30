# Effect.FS

 Practical F# effect handling with a strong focus on developer experience and .NET interop.

This repository starts with:

- a minimal cold effect core with an `effect {}` computation expression
- `Async` and `Task` interop points
- a runnable `examples/` area that covers the current feature surface practically
- a no-dependency executable test harness
- a simplified [`PLAN.md`](PLAN.md) focused on UX, interop, and effect-management goals
- an [`Effect-TS` comparison write-up](docs/EFFECT_TS_COMPARISON.md)
- a local `mise.toml` pin for `dotnet@10.0`

The implementation is intentionally small. The design center is F#-native ergonomics for managing effects, dependencies, logging, and failures without losing interoperability with ordinary .NET code.

Useful entry points:

- [`examples/README.md`](examples/README.md)
- [`docs/EFFECT_TS_COMPARISON.md`](docs/EFFECT_TS_COMPARISON.md)
