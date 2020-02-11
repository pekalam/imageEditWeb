#!/bin/bash

set -e

docker-compose -f ../E2ETests/docker/docker-compose.yml up --exit-code-from e2etests
docker-compose -f ../E2ETests/docker/docker-compose.yml down
