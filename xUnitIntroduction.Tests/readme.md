# Package Kurulumlarý


dotnet add package coverlet.collector
dotnet test --collect:"XPlat Code Coverage"
reportgenerator ` -reports:"TestResults/**/*.xml" ` -targetdir:"coverage-report" ` -reporttypes:JsonSummary
reportgenerator ` -reports:"TestResults/**/*.xml" ` -targetdir:"coverage-report" ` -reporttypes:html



Use Moq for dependencies and FluentAssertions for assertions.

Requirements:
1.	SubmitOrderRequest must not be null or empty. If null, throw ArgumentNullException.
2.	Order code must be at least 10 characters and start with "ORD". If invalid, throw Exception("Code is not Valid") and do not call Save or SendEmail.
3.	Save method of IOrderRepository must be called exactly once when order code is valid.
4.	SendEmail method of IEmailSender must be called exactly once when order code is valid.
5.	Use meaningful test method names like Handle_ShouldThrow_WhenRequestIsNull or Handle_ShouldSaveAndSendEmail_WhenOrderIsValid.
6.	Add edge test cases including:
•	Null SubmitOrderRequest
•	Empty SubmitOrderRequest
•	Order code less than 10 characters
•	Order code not starting with "ORD"
•	Order code exactly 10 characters and valid
7.	Follow Arrange-Act-Assert pattern.
8.	Include necessary using statements for Moq, FluentAssertions, and xUnit.

