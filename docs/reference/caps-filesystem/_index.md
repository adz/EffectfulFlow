---
title: "CAPS FileSystem"
---

This page shows the source-documented `FsFlow.Caps.FileSystem` surface: the file system interface and its helpers.

## Capability

- [`FileSystem.IFileSystem`](./t-filesystem-ifilesystem.md): Provides synchronous access to file system operations.

## Helpers

- [`FileSystem.FileSystem.readAllText`](./m-filesystem-filesystem-readalltext.md): Reads all text from a file using the file system environment.
- [`FileSystem.FileSystem.writeAllText`](./m-filesystem-filesystem-writealltext.md): Writes all text to a file using the file system environment.
- [`FileSystem.FileSystem.exists`](./m-filesystem-filesystem-exists.md): Checks if a file exists using the file system environment.
- [`FileSystem.FileSystem.live`](./m-filesystem-filesystem-live.md): Creates a live file system backed by <a href="https://learn.microsoft.com/dotnet/api/system.io.file">File</a>.

