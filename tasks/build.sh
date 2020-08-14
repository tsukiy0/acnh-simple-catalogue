#!/usr/bin/env bash

set -euxo pipefail

dotnet restore
dotnet build
dotnet test --filter Category=Unit
dotnet run --project ./IngestUtility/IngestUtility.csproj
dotnet publish -c Release
