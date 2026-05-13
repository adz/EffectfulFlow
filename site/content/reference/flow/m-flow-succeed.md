---
title: "Flow.succeed"
linkTitle: "succeed"
type: docs
---

<div class="fsdocs-usage">
<code><span>succeed&#32;<span>value</span></span></code>
</div>

Alias for <a href="https://learn.microsoft.com/dotnet/api/ok">ok</a> that reads well in some call sites.

## Parameters

- `value`: <code>'value</code>

## Examples

<pre class="fssnip highlighted"><code lang="fsharp"> <span class="k">let</span> <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="id">flow</span> <span class="o">=</span> <span class="id">Flow</span><span class="pn">.</span><span class="id">succeed</span> <span class="n">42</span>
 <span class="k">let</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="id">result</span> <span class="o">=</span> <span class="id">Flow</span><span class="pn">.</span><span class="id">run</span> <span class="pn">(</span><span class="pn">)</span> <span onmouseout="hideTip(event, 'fs1', 3)" onmouseover="showTip(event, 'fs1', 3)" class="id">flow</span>
 <span class="c">// result = Success 42</span>
</code></pre>
<div class="fsdocs-tip" id="fs1">val flow: obj</div>
<div class="fsdocs-tip" id="fs2">val result: obj</div>



