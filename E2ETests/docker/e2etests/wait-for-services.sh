#!/bin/sh

set -e

echo "wait-for e2e_sqlserver"
wait-for e2e_sqlserver:32112 -t 180
echo "wait-for e2e_rabbitmq"
wait-for e2e_rabbitmq:5672 -t 180
echo "wait-for-services finished"