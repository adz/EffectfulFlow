#!/usr/bin/env bash

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

cd "${ROOT_DIR}"

rm -rf "${ROOT_DIR}/.fsdocs/cache" "${ROOT_DIR}/output"

dotnet build src/EffectfulFlow/EffectfulFlow.fsproj --nologo -v minimal

dotnet fsdocs build \
    --input "${ROOT_DIR}/docs" \
    --output "${ROOT_DIR}/output" \
    --clean \
    --strict \
    --sourcefolder "${ROOT_DIR}"
