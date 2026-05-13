---
title: "Flow.fail"
linkTitle: "fail"
type: docs
---

<div class="fsdocs-usage">
<code><span>fail&#32;<span>failure</span></span></code>
</div>

Alias for <a href="https://learn.microsoft.com/dotnet/api/error">error</a> that reads well in some call sites.

## Parameters

- `failure`: <code>'error</code>

## Examples

<pre class="fssnip highlighted"><code lang="fsharp"> <span class="k">let</span> <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="id">flow</span> <span class="o">=</span> <span class="id">Flow</span><span class="pn">.</span><span class="id">fail</span> <span class="s">&quot;error&quot;</span>
 <span class="k">let</span> <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="id">result</span> <span class="o">=</span> <span class="id">Flow</span><span class="pn">.</span><span class="id">run</span> <span class="pn">(</span><span class="pn">)</span> <span onmouseout="hideTip(event, 'fs1', 3)" onmouseover="showTip(event, 'fs1', 3)" class="id">flow</span>
 <span class="c">// result = Failure (Cause.Fail &quot;error&quot;)</span>
</code></pre>
<div class="fsdocs-tip" id="fs1">val flow: obj</div>
<div class="fsdocs-tip" id="fs2">val result: obj</div>



