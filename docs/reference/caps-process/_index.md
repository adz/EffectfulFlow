---
title: "CAPS Process"
---

This page shows the source-documented `FsFlow.Caps.Process` surface: the process runner interface and its helpers.

## Capability

- [`Process.IProcess`](./t-process-iprocess.md): Provides asynchronous access to external process execution.
- [`Process.ProcessResult`](./t-process-processresult.md): Represents the outcome of an external process execution.

## Helpers

- [`Process.Process.execute`](./m-process-process-execute.md): Executes a process using the process environment and returns the result.
- [`Process.Process.live`](./m-process-process-live.md): Creates a live process runner backed by <a href="https://learn.microsoft.com/dotnet/api/system.diagnostics.process">Process</a>.

