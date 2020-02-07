#!/bin/bash

set -e

initialTimeout=20
retry=10
agentTimeout=5

if [ -e "/run/secrets/sql_server_password" ]; then
    echo "Setting password from secrets"
	SA_PASSWORD=`< /run/secrets/sql_server_password`
fi

SA_PASSWORD=$SA_PASSWORD /opt/mssql/bin/sqlservr &

/container-scripts/wait-for 0.0.0.0:1433 -t 240

echo "Starting SQL Server configuration in $initialTimeout seconds"
sleep $initialTimeout

if [ "$ENABLE_AGENT" == "yes" ]; then
    echo "Enabling SQL Server Agent"
    /opt/mssql-tools/bin/sqlcmd -b -S localhost -U sa -P $SA_PASSWORD -i /container-scripts/setup-agent.sql
    echo "SQL Server Agent enabled"
fi

echo "Waiting for SQL Server Agent to start (retry count = $retry, timeout = $agentTimeout)"
while [ $retry -gt 0 ]; do
	result=$(/opt/mssql-tools/bin/sqlcmd -b -S localhost -U sa -P $SA_PASSWORD -Q "exec msdb.dbo.sp_is_sqlagent_starting" 1>/dev/null 2>/dev/null && echo $? || echo $?)
	if [ $result -eq 0 ]; then
		break
	fi
	retry=$(($retry-1))
	sleep $agentTimeout
done

if [ $retry -eq 0 ]; then
	echo "Cannot start SQL Server Agent"
	exit 1
fi
echo "SQL Server Agent started"

if [ -e "/container-scripts/init.sql" ]; then
	echo "Running init.sql"
	/opt/mssql-tools/bin/sqlcmd -b -S localhost -U sa -P $SA_PASSWORD -i /container-scripts/init.sql
fi

echo "Starting to listen on health port"
/container-scripts/listen-on-health-port.sh &

echo "SQL Server started"

wait