#!/bin/bash
password=SqlServer2019!
wait_time=30s

# Esperar até que o SQL Server esteja pronto para aceitar conexões
echo "Aguardando 30s..."
sleep $wait_time
echo "Executando script..."

# run the init script to create the DB and the tables
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P $password -d master -i /tmp/setup.sql
