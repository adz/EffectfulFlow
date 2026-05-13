---
title: "Flow.fromResult"
linkTitle: "fromResult"
type: docs
---

<div class="fsdocs-usage">
<code><span>fromResult&#32;<span>result</span></span></code>
</div>

Lifts a <a href="https://learn.microsoft.com/dotnet/api/system.result-2">Result</a> into a synchronous flow.

## Parameters

- `result`: <code><span><a href="https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-fsharpresult-2">Result</a>&lt;<span>'value,&#32;'error</span>&gt;</span></code>

## Examples

<pre class="fssnip highlighted"><code lang="fsharp"> <span class="k">let</span> <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="id">res</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="uc">Ok</span> <span class="s">&quot;success&quot;</span>
 <span class="k">let</span> <span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="id">flow</span> <span class="o">=</span> <span class="id">Flow</span><span class="pn">.</span><span class="id">fromResult</span> <span onmouseout="hideTip(event, 'fs1', 4)" onmouseover="showTip(event, 'fs1', 4)" class="id">res</span>
</code></pre>
<div class="fsdocs-tip" id="fs1">val res: Result&lt;string,&#39;a&gt;</div>
<div class="fsdocs-tip" id="fs2">union case Result.Ok: ResultValue: &#39;T -&gt; Result&lt;&#39;T,&#39;TError&gt;</div>
<div class="fsdocs-tip" id="fs3">val flow: obj</div>



