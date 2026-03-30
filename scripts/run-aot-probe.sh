#!/usr/bin/env bash

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT="$ROOT_DIR/examples/EffectFs.AotProbe/EffectFs.AotProbe.fsproj"
PUBLISH_DIR="$ROOT_DIR/artifacts/publish/EffectFs.AotProbe/linux-x64"

dotnet publish "$PROJECT" -c Release -r linux-x64 -p:PublishAot=true -o "$PUBLISH_DIR"
"$PUBLISH_DIR/EffectFs.AotProbe"
