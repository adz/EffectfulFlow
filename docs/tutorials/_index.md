---
weight: 15
title: Tutorials
description: Step-by-step guides for common FsFlow patterns.
---

# Tutorials

These tutorials provide step-by-step setups for common FsFlow dependency management patterns.

## Which Tutorial Should I Use?

| Tutorial | Focus | Best For |
| :--- | :--- | :--- |
| **[AppRecord](./app-record/)** | Simple concrete records | Smaller apps, direct field access, and low boilerplate. |
| **[Capabilities](./capabilities/)** | App capability contracts | Reusable helpers, large-scale apps, and type-checked app dependencies. Runtime services stay implicit and testable. |
| **[AppHost](./app-host/)** | .NET Generic Host / DI | Integration with ASP.NET Core, Worker Services, and existing DI containers. |

Each tutorial builds on the previous concepts. If you are new to FsFlow, start with **AppRecord**.
