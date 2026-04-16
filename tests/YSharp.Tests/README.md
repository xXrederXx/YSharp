# Test

## How to run

You can use the Powershell script `test.ps1`, it supports:

- `-d` to delete the TestResults Folder
- `-o` to automaticaly open the report


Use the following command to run the tests, if you cant use the ps1 script:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

If needed you can generate a report using this command:

```bash
reportgenerator -reports:TestResults/**/coverage.cobertura.xml -targetdir:TestResults/coverage-report
```

(Install report generator if needed)

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```
