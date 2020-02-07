#!/bin/bash

set -e


dotnet test ../UnitTests/UnitTests.csproj -p:CollectCoverage=true -p:MergeWith="../IntegrationTests/docker-testenv/testReports/coverage.json" /maxcpucount:8 -p:CoverletOutput="../UnitTests/testReports/coverage" -p:CoverletOutputFormat="opencover"