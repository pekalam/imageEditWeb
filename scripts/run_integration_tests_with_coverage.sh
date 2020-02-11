#!/bin/bash

set -e

if [ -e "../UnitTests/testReports/coverage.json" ]; then
    cp "../UnitTests/testReports/coverage.json" "../IntegrationTests/docker/testReports"
else
    echo "cannot find ../UnitTests/testReports/coverage.json"
    exit 1
fi

docker-compose -f ../IntegrationTests/docker/docker-compose.yml up --exit-code-from integrationtests
docker-compose -f ../IntegrationTests/docker/docker-compose.yml down