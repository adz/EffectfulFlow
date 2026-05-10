---
weight: 30
title: Validate CE
description: Accumulating validation with the validate { } builder.
---

# Validate CE

Use the `validate {}` computation expression when you have multiple independent checks and you want to **collect every failure** into a single report.

This is "accumulating" semantics.

## Accumulating with `and!`

The key to accumulation is the `and!` keyword. Steps joined by `and!` are evaluated independently, and their errors are merged into a `Diagnostics` graph.

```fsharp
type Registration = { Name: string; Email: string }
type RegError = NameRequired | EmailRequired

let validateRegistration input =
    validate {
        let! name = input.Name |> Check.notBlank |> Check.orError NameRequired
        and! email = input.Email |> Check.notBlank |> Check.orError EmailRequired
        return { Name = name; Email = email }
    }

let outcome = validateRegistration { Name = ""; Email = "" }
// outcome = Validation (Error {
//   Errors = [NameRequired; EmailRequired]
//   Children = []
// })
```

If both fields are blank, the result contains a `Diagnostics` object with both `NameRequired` and `EmailRequired`.

## Sequential Steps in `validate {}`

Standard `let!` and `do!` inside a `validate {}` block still short-circuit. This is useful for "gate" checks that must pass before other validation can proceed.

```fsharp
validate {
    // Stop immediately if the whole object is null
    let! input = input |> Check.notNull |> Check.orError ObjectMissing
    
    // These run only if input was not null
    let! name = input.Name |> Check.notBlank |> Check.orError NameRequired
    and! email = input.Email |> Check.notBlank |> Check.orError EmailRequired
    
    return { Name = name; Email = email }
}
```

## Nested Scopes

To build a structured report (e.g., for JSON APIs), use the `validate.key`, `validate.index`, and `validate.name` helpers. These prefix any diagnostics produced inside the block.

```fsharp
let validateCustomer customer =
    validate.key "customer" {
        let! name = 
            validate.name "Name" {
                return! customer.Name |> Check.notBlank |> Check.orError "Required"
            }
        return name
    }

let v = validateCustomer { Name = "" }
// v = Validation (Error {
//   Errors = []
//   Children = [
//     Key "customer" -> {
//       Errors = []
//       Children = [
//         Name "Name" -> { Errors = ["Required"]; Children = [] }
//       ]
//     }
//   ]
// })
```

Using `Diagnostics.toString v` would render:
```text
customer:
  Name:
  - Required
```

## When to use `validate {}`

- **Forms and User Input**: Where the user wants to see all errors at once.
- **Complex Documents**: Where you need to point failures back to specific paths or indices.
- **Independent Rules**: When rules can be checked in any order.

To learn more about the structure of the accumulated errors, see [Diagnostics Graph](../diagnostics/).
