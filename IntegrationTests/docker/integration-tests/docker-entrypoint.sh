#!/bin/bash

set -e
export USE_SETTINGS="Y"
wait-for-services.sh && dotnet test --no-build IntegrationTests.csproj /p:CollectCoverage=true -p:MergeWith="/reports/coverage.json" /maxcpucount:8 /p:CoverletOutput="/reports/coverage" -p:CoverletOutputFormat="opencover"