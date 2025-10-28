# Package Kurulumlarý


dotnet add package coverlet.collector
dotnet test --collect:"XPlat Code Coverage"
reportgenerator ` -reports:"TestResults/**/*.xml" ` -targetdir:"coverage-report" ` -reporttypes:JsonSummary
reportgenerator ` -reports:"TestResults/**/*.xml" ` -targetdir:"coverage-report" ` -reporttypes:html
