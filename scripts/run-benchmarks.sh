#!/usr/bin/env bash

set -euo pipefail

read -r -p "Stop other processes and run the .NET benchmark suite? [y/N] " answer

case "$answer" in
  y|Y|yes|YES)
    dotnet run --project benchmarks/FsFlow.Benchmarks/FsFlow.Benchmarks.fsproj --nologo
    ;;
  *)
    echo "Benchmark run cancelled."
    exit 1
    ;;
esac
