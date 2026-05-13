---
title: "Schedule"
---

The `Schedule` module provides a DSL for describing execution policies.

## Core type

- [`Schedule`](./t-schedule-3.md):  Represents a stateful schedule that can decide whether to continue and how long to delay.

## Module functions

- [`Schedule.recurs`](./m-schedule-recurs.md): Creates a schedule that recurs a fixed number of times.
- [`Schedule.spaced`](./m-schedule-spaced.md): Creates a schedule that recurs with a fixed delay between attempts.
- [`Schedule.exponential`](./m-schedule-exponential.md): Creates a schedule that recurs with exponential backoff.
- [`Schedule.jittered`](./m-schedule-jittered.md): Adds random jitter to a schedule's delay.

## Flow extensions

- [`FlowSchedule.Retry`](./m-flowschedule-retry.md): Retries a failing flow according to the supplied schedule.
- [`FlowSchedule.Repeat`](./m-flowschedule-repeat.md): Repeats a successful flow according to the supplied schedule.

