---
title: "validate"
linkTitle: "validate { }"
type: docs
---

<div class="fsdocs-usage">
<code><span>validate&#32;<span></span></span></code>
</div>


 The accumulating <code>validate { }</code> computation expression.
 

## Remarks

<p class='fsdocs-para'>
 Use this builder when you want to collect all validation failures instead of stopping
 at the first one.
 </p><p class='fsdocs-para'>
 Use <code>and!</code> when sibling validations should accumulate into the same diagnostics graph.
 Plain <code>let!</code> and <code>do!</code> are sequential: if the left side fails, the later step is
 not evaluated.
 </p><p class='fsdocs-para'><code>Check&lt;&#39;value&gt;</code> covers both value-preserving checks and gate checks.
 Use <code>Check.orError</code> to attach an application error, and <code>Guard.Of</code> /
 <code>Guard.MapError</code> when you want the same error-bound source shape to participate
 directly in validation.
 </p><p class='fsdocs-para'>
 When nested API response fields need to keep their place in the diagnostics graph, use
 the scoped helpers <code>validate.key</code>, <code>validate.index</code>, and <code>validate.name</code>
 inside the computation expression. If you already have a <code>Validation</code> value, use
 <code>Validation.key</code>, <code>Validation.index</code>, or <code>Validation.name</code> to prefix it
 after the fact.
 </p><p class='fsdocs-para'>
 It is intended for forms, configuration checks, and other input-heavy boundaries where
 the user benefits from seeing every problem at once.
 </p>

## Examples


 ```fsharp
 let validatedUser =
     validate {
         let! name = Check.notBlank input.Name
         let! age = Check.okIf (input.Age &gt; 0) &quot;Age must be positive&quot;
         return { Name = name; Age = age }
     }
 ```

 ```fsharp
 let validatedCustomer =
     validate.key &quot;customer&quot; {
         let! name =
             validate.name &quot;Name&quot; {
                 return! input.Name |&gt; Check.notBlank |&gt; Check.orError &quot;Name required&quot;
             }

         return name
     }
 ```
 

