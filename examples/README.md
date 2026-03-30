# Examples

The `examples` area should read like a getting-started guide for ordinary F# application code.

Run the example with:

```bash
dotnet run --project examples/EffectFs.Examples/EffectFs.Examples.fsproj --nologo
```

Current example:

- validate configuration with plain `Result`
- lift validation into `Effect`
- read dependencies from environment with `Effect.ask` and `Effect.read`
- call async and task-based work through the effect boundary
- convert legacy exceptions into typed errors

The point of the example is not architecture. The point is to show what normal effectful F# code feels like with this library.
