---
title: "result"
linkTitle: "result { }"
---

<div class="fsdocs-usage">
<code><span>result&#32;<span></span></span></code>
</div>


 The fail-fast <code>result { }</code> computation expression.
 

## Remarks

<p class='fsdocs-para'>
 Use this builder when the happy path should short-circuit on the first error
 and you want to keep the workflow in <code>Result</code> shape all the way through.
 </p><p class='fsdocs-para'>
 It works well for parsing, validation, and other boundaries where failure is expected
 to stop the flow immediately instead of accumulating diagnostics.
 </p><p class='fsdocs-para'>
 Use <code>Check.orError</code> when a pure check needs a domain error, and <code>Guard.MapError</code> when
 you need to remap an existing error before entering the CE.
 </p>

## Examples


 ```fsharp
 let parsedUser =
     result {
         let! age = parseAge input
         let! name = parseName input
         return { Age = age; Name = name }
     }
 ```
 

