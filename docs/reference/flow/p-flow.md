---
title: "flow"
linkTitle: "flow { }"
---

<div class="fsdocs-usage">
<code><span>flow&#32;<span></span></span></code>
</div>


 The universal <code>flow { }</code> computation expression.
 

## Remarks

<p class='fsdocs-para'>
 Use this builder when the boundary can mix synchronous values, <code>Async</code>, <code>Task</code>,
 <code>Result</code>, and environment requests while keeping typed failures and explicit
 dependency access.
 </p><p class='fsdocs-para'>
 It preserves the current environment model while allowing the workflow to compose
 task-oriented inputs directly, so callers do not need to switch builders just to cross
 an async boundary.
 </p>

## Examples


 ```fsharp
 let greeting =
     flow {
         let! name = Flow.env
         let! suffix = async { return &quot;!&quot; }
         return $&quot;Hello, {name}{suffix}&quot;
     }
 ```
 

