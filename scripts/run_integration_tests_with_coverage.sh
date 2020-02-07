#!/bin/bash

set -e

docker-compose -f ../IntegrationTests/docker-testenv/docker-compose.yml up --exit-code-from integrationtests
docker-compose -f ../IntegrationTests/docker-testenv/docker-compose.yml down