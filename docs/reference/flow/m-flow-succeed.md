---
title: "Flow.succeed"
linkTitle: "succeed"
---

<div class="fsdocs-usage">
<code><span>succeed&#32;<span>value</span></span></code>
</div>

Alias for <code>ok</code> that reads well in some call sites.

## Parameters

- `value`: <code>'value</code>

## Examples

<pre class="fssnip highlighted"><code lang="fsharp"> <span class="k">let</span> <span data-fsdocs-tip="fs1" data-fsdocs-tip-unique="1" class="id">flow</span> <span class="o">=</span> <span class="id">Flow</span><span class="pn">.</span><span class="id">succeed</span> <span class="n">42</span>
 <span class="k">let</span> <span data-fsdocs-tip="fs2" data-fsdocs-tip-unique="2" class="id">result</span> <span class="o">=</span> <span class="id">Flow</span><span class="pn">.</span><span class="id">run</span> <span class="pn">(</span><span class="pn">)</span> <span data-fsdocs-tip="fs1" data-fsdocs-tip-unique="3" class="id">flow</span>
 <span class="c">// result = Success 42</span>
</code></pre>
<div popover class="fsdocs-tip" id="fs1">val flow: obj</div>
<div popover class="fsdocs-tip" id="fs2">val result: obj</div>



