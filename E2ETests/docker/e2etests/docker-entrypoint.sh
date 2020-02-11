#!/bin/bash

set -e
export USE_SETTINGS="Y"
wait-for-services.sh && dotnet test --no-build E2ETests.csproj