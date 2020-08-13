#!/usr/bin/env bash

set -euxo pipefail

dotnet restore
dotnet publish -c Release
