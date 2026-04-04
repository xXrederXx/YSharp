# Test

## How to run

Use the following command to run the tests:

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
