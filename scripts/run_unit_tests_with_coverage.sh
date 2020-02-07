#!/bin/bash

set -e


dotnet test ../UnitTests/UnitTests.csproj -p:CollectCoverage=true -p:MergeWith="../testReports/coverage.json" /maxcpucount:8 -p:CoverletOutput="../testReports/coverage" -p:CoverletOutputFormat="opencover"