#!/bin/sh

set -e

echo "wait-for sql_server"
wait-for ig_sqlserver:32112 -t 180
echo "wait-for ig_rabbitmq"
wait-for ig_rabbitmq:5672 -t 180
echo "mongodb started"