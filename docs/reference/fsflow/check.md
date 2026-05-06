---
title: Check
description: Source-documented pure predicate helpers for FsFlow.
---

# Check

This page shows the source-documented `Check` surface: the unit-failure result type and the reusable predicate helpers.

## Core type

- type `Check`: A reusable predicate result that either preserves a value on success or acts as a gate with
`unit` on success, while carrying a unit failure placeholder until the caller maps it into
a domain-specific error. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L425)

## Module functions

- module `Check`: Predicate helpers that return `Result` values with a unit error,
plus the bridge functions that turn those checks into application errors. Some helpers preserve
the source value; others are gates and return `unit` on success. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L434)
- [`Check.fromPredicate`](./check-frompredicate.md): Builds a check from a predicate while preserving the successful value. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L439)
- [`Check.not`](./check-not.md): Returns success when the supplied check fails. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L452)
- [`Check.and`](./check-and.md): Returns success when both checks succeed. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L465)
- [`Check.or`](./check-or.md): Returns success when either check succeeds. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L481)
- [`Check.all`](./check-all.md): Returns success when every check in the sequence succeeds. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L496)
- [`Check.any`](./check-any.md): Returns success when at least one check in the sequence succeeds. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L518)
- [`Check.okIf`](./check-okif.md): Returns success when the condition is true. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L536)
- [`Check.failIf`](./check-failif.md): Returns success when the condition is false. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L542)
- [`Check.okIfSome`](./check-okifsome.md): Returns the value when the option is `Some`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L548)
- [`Check.okIfNone`](./check-okifnone.md): Returns success when the option is `None`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L556)
- [`Check.failIfSome`](./check-failifsome.md): Returns success when the option is `None`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L564)
- [`Check.failIfNone`](./check-failifnone.md): Returns the value when the option is `Some`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L572)
- [`Check.okIfValueSome`](./check-okifvaluesome.md): Returns the value when the value option is `ValueSome`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L580)
- [`Check.okIfValueNone`](./check-okifvaluenone.md): Returns success when the value option is `ValueNone`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L588)
- [`Check.failIfValueSome`](./check-failifvaluesome.md): Returns success when the value option is `ValueNone`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L596)
- [`Check.failIfValueNone`](./check-failifvaluenone.md): Returns the value when the value option is `ValueSome`. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L604)
- [`Check.okIfNotNull`](./check-okifnotnull.md): Returns the value when it is not null. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L612)
- [`Check.okIfNull`](./check-okifnull.md): Returns success when the value is null. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L618)
- [`Check.failIfNotNull`](./check-failifnotnull.md): Returns success when the value is null. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L624)
- [`Check.failIfNull`](./check-failifnull.md): Returns the value when it is null. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L630)
- [`Check.okIfNotEmpty`](./check-okifnotempty.md): Returns the sequence when it is not empty. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L636)
- [`Check.okIfEmpty`](./check-okifempty.md): Returns success when the sequence is empty. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L642)
- [`Check.failIfNotEmpty`](./check-failifnotempty.md): Returns success when the sequence is empty. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L648)
- [`Check.failIfEmpty`](./check-failifempty.md): Returns the sequence when it is not empty. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L654)
- [`Check.okIfEqual`](./check-okifequal.md): Returns success when the values are equal. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L661)
- [`Check.okIfNotEqual`](./check-okifnotequal.md): Returns success when the values are not equal. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L668)
- [`Check.failIfEqual`](./check-failifequal.md): Returns success when the values are equal. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L675)
- [`Check.failIfNotEqual`](./check-failifnotequal.md): Returns success when the values are not equal. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L682)
- [`Check.okIfNonEmptyStr`](./check-okifnonemptystr.md): Returns the string when it is not null or empty. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L688)
- [`Check.okIfEmptyStr`](./check-okifemptystr.md): Returns success when the string is null or empty. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L694)
- [`Check.failIfNonEmptyStr`](./check-failifnonemptystr.md): Returns success when the string is null or empty. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L700)
- [`Check.failIfEmptyStr`](./check-failifemptystr.md): Returns the string when it is null or empty. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L706)
- [`Check.okIfNotBlank`](./check-okifnotblank.md): Returns the string when it is not blank. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L712)
- [`Check.notBlank`](./check-notblank.md): Returns the string when it is not blank. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L718)
- [`Check.okIfBlank`](./check-okifblank.md): Returns success when the string is blank. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L724)
- [`Check.blank`](./check-blank.md): Returns success when the string is blank. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L730)
- [`Check.failIfNotBlank`](./check-failifnotblank.md): Returns success when the string is blank. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L736)
- [`Check.failIfBlank`](./check-failifblank.md): Returns the string when it is blank. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L742)
- [`Check.orError`](./check-orerror.md): Maps a unit error into the supplied application error value. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L778)
- [`Check.orErrorWith`](./check-orerrorwith.md): Maps a unit error into an application error produced on demand. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L787)
- [`Check.notNull`](./check-notnull.md): Returns the value when it is not null. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L748)
- [`Check.notEmpty`](./check-notempty.md): Returns the sequence when it is not empty. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L754)
- [`Check.equal`](./check-equal.md): Returns success when the values are equal. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L761)
- [`Check.notEqual`](./check-notequal.md): Returns success when the values are not equal. [source](https://github.com/adz/FsFlow/blob/main/src/FsFlow/Validate.fs#L768)

