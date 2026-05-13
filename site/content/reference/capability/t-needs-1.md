---
title: "Needs"
linkTitle: "Needs<dep>"
type: docs
---

Describes the capability contract for a single dependency.

## Remarks


 Named cap-set interfaces inherit this contract once and then expose the dependency through a
 member such as <code>Clock</code> or <code>Logger</code>. Workflow builders can accept any environment
 that implements <code>Needs&lt;&#39;dep&gt;</code>, which lets larger runtimes satisfy smaller
 boundaries.
 
