#!/bin/bash

set -e


#dotnet test ../UnitTests/UnitTests.csproj -p:CollectCoverage=true -p:MergeWith="../IntegrationTests/docker/testReports/coverage.json" /maxcpucount:8 -p:CoverletOutput="../UnitTests/testReports/coverage" -p:CoverletOutputFormat="opencover"



dotnet test ../UnitTests/UnitTests.csproj -p:CollectCoverage=true /maxcpucount:8 -p:CoverletOutput="../UnitTests/testReports/coverage"
