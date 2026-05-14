---
title: "Resolve"
linkTitle: "Resolve<dep>"
type: docs
---

Request token for binding a whole dependency inside a workflow.

## Remarks


 Use this token when a workflow needs the dependency itself rather than a value projected from
 that dependency. The <code>flow {}</code> builder and its compatibility helpers read it from any
 environment that exposes the dependency directly or through a nominal capability contract.
 
