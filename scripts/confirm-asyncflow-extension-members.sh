#!/usr/bin/env bash

set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
probe_root="$(mktemp -d)"
trap 'rm -rf "$probe_root"' EXIT
sdk_root="/home/adam/.local/share/mise/installs/dotnet/10.0.201/sdk/10.0.201/FSharp"
fsc_dll="$sdk_root/fsc.dll"
fsflow_dll="$repo_root/artifacts/bin/FsFlow/debug_net8.0/FsFlow.dll"

core_only_script="$probe_root/core-only.fsx"
extension_source="$probe_root/Extensions.fs"
extension_dll="$probe_root/with-extension.dll"
consumer_script="$probe_root/consumer.fsx"

cat > "$core_only_script" <<EOF
#r @"$fsflow_dll"
open System.Threading.Tasks
open FsFlow

let probe : AsyncFlow<unit, string, int> =
    asyncFlow {
        let! value = Task.FromResult 42
        return value
    }
EOF

cat > "$extension_source" <<'EOF'
namespace Probe.WithExtension

open System.Threading.Tasks
open FsFlow

[<AutoOpen>]
module AsyncFlowTaskExtensions =
    type AsyncFlowBuilder with
        member _.Bind
            (
                task: Task<'value>,
                binder: 'value -> AsyncFlow<'env, 'error, 'next>
            ) : AsyncFlow<'env, 'error, 'next> =
            task
            |> Async.AwaitTask
            |> AsyncFlow.fromAsync
            |> AsyncFlow.bind binder
EOF

dotnet "$fsc_dll" \
    --targetprofile:netcore \
    -a \
    -o:"$extension_dll" \
    -r:"$fsflow_dll" \
    "$extension_source" \
    >/dev/null

cat > "$consumer_script" <<EOF
#r @"$fsflow_dll"
#r @"$extension_dll"
open System.Threading.Tasks
open FsFlow
open Probe.WithExtension

let probe : AsyncFlow<unit, string, int> =
    asyncFlow {
        let! value = Task.FromResult 42
        return value
    }
EOF

set +e
core_only_output="$(dotnet fsi "$core_only_script" 2>&1)"
core_only_status=$?
set -e

if [ "$core_only_status" -eq 0 ]; then
    printf '%s\n' "$core_only_output"
    echo "Expected the core-only probe to fail, but it succeeded."
    exit 1
fi

printf '%s\n' "$core_only_output"

dotnet fsi "$consumer_script" >/dev/null

echo "Confirmed: AsyncFlowBuilder extension members are enough to add task binds only from a second assembly."
