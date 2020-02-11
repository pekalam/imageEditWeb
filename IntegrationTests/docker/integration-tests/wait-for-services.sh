#!/bin/sh

set -e

echo "wait-for ig_sqlserver"
wait-for ig_sqlserver:32112 -t 180
echo "wait-for ig_rabbitmq"
wait-for ig_rabbitmq:5672 -t 180
echo "wait-for-services finished"
