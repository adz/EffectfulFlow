---
title: "Ref"
---

The `Ref` module provides thread-safe mutable state handles.

## Core type

- [`Ref`](./t-ref-1.md): 
 Represents a handle to a mutable reference that can be updated atomically.
 

## Module functions

- [`Ref.make`](./m-ref-make.md): Creates a new <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-ref-1.html">Ref</a> with the initial value.
- [`Ref.get`](./m-ref-get.md): Reads the current value of the reference.
- [`Ref.set`](./m-ref-set.md): Sets the value of the reference to the specified value.
- [`Ref.update`](./m-ref-update.md): Updates the value of the reference using the supplied function.
- [`Ref.modify`](./m-ref-modify.md): Updates the value of the reference using the supplied function and returns a derived value.

