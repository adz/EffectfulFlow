---
title: "Diagnostics"
linkTitle: "Diagnostics<error>"
---


 A mergeable validation graph that carries local errors and nested child branches.
 

## Remarks

<p class='fsdocs-para'><code>Errors</code> holds the application errors attached exactly to the current node, while
 <code>Children</code> holds nested branches keyed by <a href="https://adz.github.io/FsFlow/reference/FsFlow/fsflow-pathsegment.html">PathSegment</a>.
 </p><p class='fsdocs-para'>
 This structure allows hierarchical validation to stay navigable before flattening.
 Use <a href="https://learn.microsoft.com/dotnet/api/fsflow.diagnostics.flatten">flatten</a> to convert it into a linear list.
 </p>