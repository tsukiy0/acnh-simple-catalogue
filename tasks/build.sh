#!/usr/bin/env bash

set -euxo pipefail

dotnet restore
dotnet public -c Release
