@echo off
set THISDIR=%~dp0
set SQLDIR=%THISDIR%sql
set DBPRJ_DIR=%THISDIR%..\Mirero.DAQ.Infrastructure
set ACCOUNT_APIPRJ_DIR=%THISDIR%..\Mirero.DAQ.Service.Api.Account
set OFFLINE_APIPRJ_DIR=%THISDIR%..\Mirero.DAQ.Service.Api.Offline
if not exist "%SQLDIR%" mkdir "%SQLDIR%"
echo Building and Checking DB Context List
echo SQLDIR : %SQLDIR%
dotnet ef dbcontext list -p "%DBPRJ_DIR%" -s "%ACCOUNT_APIPRJ_DIR%"
dotnet ef dbcontext list -p "%DBPRJ_DIR%" -s "%OFFLINE_APIPRJ_DIR%"
dotnet ef dbcontext script --no-build -p "%DBPRJ_DIR%"  -s "%ACCOUNT_APIPRJ_DIR%" -c AccountDbContextPostgreSQL -o "%SQLDIR%\account-db.sql" -- --service Api.Account
dotnet ef dbcontext script --no-build -p "%DBPRJ_DIR%"  -s "%OFFLINE_APIPRJ_DIR%" -c DatasetDbContextPostgreSQL -o "%SQLDIR%\dataset-db.sql" -- --service Api.Offline
dotnet ef dbcontext script --no-build -p "%DBPRJ_DIR%"  -s "%OFFLINE_APIPRJ_DIR%" -c InferenceDbContextPostgreSQL -o "%SQLDIR%\inference-db.sql" -- --service Api.Offline
dotnet ef dbcontext script --no-build -p "%DBPRJ_DIR%"  -s "%OFFLINE_APIPRJ_DIR%" -c GdsDbContextPostgreSQL -o "%SQLDIR%\gds-db.sql" -- --service Api.Offline
dotnet ef dbcontext script --no-build -p "%DBPRJ_DIR%"  -s "%OFFLINE_APIPRJ_DIR%" -c WorkflowDbContextPostgreSQL -o "%SQLDIR%\workflow-db.sql" -- --service Api.Offline