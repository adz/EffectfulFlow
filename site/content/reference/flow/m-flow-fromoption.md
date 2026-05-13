---
title: "Flow.fromOption"
linkTitle: "fromOption"
type: docs
---

<div class="fsdocs-usage">
<code><span>fromOption&#32;<span>error&#32;value</span></span></code>
</div>

Lifts an option into a synchronous flow with the supplied error.

## Parameters

- `error`: <code>'error</code>
- `value`: <code><span>'value&#32;option</span></code>

## Examples

<pre class="fssnip highlighted"><code lang="fsharp"> <span class="k">let</span> <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="id">opt</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="uc">Some</span> <span class="s">&quot;value&quot;</span>
 <span class="k">let</span> <span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="id">flow</span> <span class="o">=</span> <span class="id">Flow</span><span class="pn">.</span><span class="id">fromOption</span> <span class="s">&quot;missing&quot;</span> <span onmouseout="hideTip(event, 'fs1', 4)" onmouseover="showTip(event, 'fs1', 4)" class="id">opt</span>
</code></pre>
<div class="fsdocs-tip" id="fs1">val opt: string option</div>
<div class="fsdocs-tip" id="fs2">union case Option.Some: Value: &#39;T -&gt; Option&lt;&#39;T&gt;</div>
<div class="fsdocs-tip" id="fs3">val flow: obj</div>



