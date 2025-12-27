@echo off

REM Get the current Git commit hash
for /f %%H in ('git rev-parse --verify HEAD') do set HASH=%%H

REM Run the dotnet app in Release configuration, passing the hash
dotnet run -c Release -- %HASH%
