using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using System.Text.RegularExpressions;
using Xunit;

namespace XUnitIntroduction.Tests;

/// <summary>
/// Comprehensive test suite demonstrating advanced FluentAssertions usage with xUnit
/// </summary>
public class AdvancedValidationTests
{
    #region Test Services and Dependencies

    // Interface for validator service (to be mocked)
    public interface IValidationLogger
    {
        void LogValidation(string fieldName, bool isValid);
        void LogError(string message);
    }

    // Service class to test
    public class ValidationService
    {
        private readonly IValidationLogger _logger;

        public ValidationService(IValidationLogger logger)
        {
            _logger = logger;
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                _logger.LogError("Password cannot be null or empty");
                return false;
            }

            var isValid = password.Length >= 8 &&
                          Regex.IsMatch(password, @"[A-Z]") &&
                          Regex.IsMatch(password, @"[a-z]") &&
                          Regex.IsMatch(password, @"\d") &&
                          Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]");

            _logger.LogValidation("Password", isValid);
            return isValid;
        }

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                _logger.LogError("Phone number cannot be null or empty");
                return false;
            }

            // Supports formats: +905551234567, (555) 123-4567, 555-123-4567
            var patterns = new[]
            {
                @"^\+90\d{10}$",                    // International Turkish
                @"^\(\d{3}\)\s?\d{3}-\d{4}$",       // US format with parentheses
                @"^\d{3}-\d{3}-\d{4}$"              // Standard dashed format
            };

            var isValid = patterns.Any(pattern => Regex.IsMatch(phoneNumber, pattern));
            _logger.LogValidation("PhoneNumber", isValid);
            return isValid;
        }

        public double CalculateRiskScore(double value)
        {
            _logger.LogValidation("RiskScore", !double.IsNaN(value));
            return value * 1.5;
        }
    }

    #endregion

    #region Password Validation Tests

    [Theory]
    [MemberData(nameof(ValidPasswordData))]
    public void ValidatePassword_WithValidPasswords_ShouldReturnTrue(string password, string description)
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        var service = new ValidationService(mockLogger.Object);

        // Act
        var result = service.ValidatePassword(password);

        // Assert
        result.Should().BeTrue(because: $"password '{description}' meets all requirements");
        
        password.Should()
            .NotBeNullOrEmpty()
            .And.HaveLength(c => c >= 8, "password must be at least 8 characters")
            .And.MatchRegex(@"[A-Z]", "password must contain uppercase letter")
            .And.MatchRegex(@"[a-z]", "password must contain lowercase letter")
            .And.MatchRegex(@"\d", "password must contain digit")
            .And.MatchRegex(@"[!@#$%^&*(),.?""':{}|<>]", "password must contain special character");

        mockLogger.Verify(x => x.LogValidation("Password", true), Times.Once);
    }

    [Theory]
    [ClassData(typeof(InvalidPasswordTestData))]
    public void ValidatePassword_WithInvalidPasswords_ShouldReturnFalse(string password, string reason)
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        var service = new ValidationService(mockLogger.Object);

        // Act
        var result = service.ValidatePassword(password);

        // Assert
        result.Should().BeFalse(because: reason);
        
        if (string.IsNullOrEmpty(password))
        {
            mockLogger.Verify(x => x.LogError("Password cannot be null or empty"), Times.Once);
        }
        else
        {
            mockLogger.Verify(x => x.LogValidation("Password", false), Times.Once);
        }
    }

    public static IEnumerable<object[]> ValidPasswordData()
    {
        yield return new object[] { "MyPass123!", "contains all required character types" };
        yield return new object[] { "Str0ng@Pass", "has special character @ and meets all criteria" };
        yield return new object[] { "C0mplex#2024", "includes year and special characters" };
    }

    #endregion

    #region Phone Number Validation Tests

    [Theory]
    [InlineData("+905551234567", "Turkish international format")]
    [InlineData("(555) 123-4567", "US format with parentheses and space")]
    [InlineData("(555)123-4567", "US format with parentheses without space")]
    [InlineData("555-123-4567", "Standard dashed format")]
    public void ValidatePhoneNumber_WithValidFormats_ShouldReturnTrue(string phoneNumber, string format)
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        var service = new ValidationService(mockLogger.Object);

        // Act
        var result = service.ValidatePhoneNumber(phoneNumber);

        // Assert
        result.Should().BeTrue(because: $"phone number matches {format}");
        
        phoneNumber.Should()
            .NotBeNullOrWhiteSpace()
            .And.Satisfy(
                p => Regex.IsMatch(p, @"^\+90\d{10}$") ||
                     Regex.IsMatch(p, @"^\(\d{3}\)\s?\d{3}-\d{4}$") ||
                     Regex.IsMatch(p, @"^\d{3}-\d{3}-\d{4}$"),
                "it matches at least one valid phone pattern");

        mockLogger.Verify(x => x.LogValidation("PhoneNumber", true), Times.Once);
    }

    [Theory]
    [MemberData(nameof(InvalidPhoneNumberData))]
    public void ValidatePhoneNumber_WithInvalidFormats_ShouldReturnFalse(string phoneNumber)
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        var service = new ValidationService(mockLogger.Object);

        // Act
        var result = service.ValidatePhoneNumber(phoneNumber);

        // Assert
        result.Should().BeFalse();
        phoneNumber.Should().NotMatchRegex(@"^\+90\d{10}$");
    }

    public static IEnumerable<object[]> InvalidPhoneNumberData()
    {
        yield return new object[] { "12345" };
        yield return new object[] { "+9055" };
        yield return new object[] { "invalid-phone" };
        yield return new object[] { "(555) 12-456" };
    }

    #endregion

    #region Regex Pattern Assertions

    [Fact]
    public void EmailPattern_ShouldMatchValidEmails()
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        var validEmails = new[] { "user@example.com", "test.user@domain.co.uk", "admin@test-site.org" };
        var emailPattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";

        // Act & Assert
        validEmails.Should()
            .AllSatisfy(email => email.Should().MatchRegex(emailPattern))
            .And.HaveCount(3)
            .And.OnlyContain(email => email.Contains("@"));

        mockLogger.Object.Should().NotBeNull();
    }

    [Theory]
    [InlineData("ABC-123", @"^[A-Z]{3}-\d{3}$")]
    [InlineData("2024-10-27", @"^\d{4}-\d{2}-\d{2}$")]
    [InlineData("#FF5733", @"^#[0-9A-F]{6}$")]
    public void CustomPatterns_ShouldMatchExpectedFormats(string input, string pattern)
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();

        // Act & Assert
        input.Should()
            .MatchRegex(pattern)
            .And.NotBeNullOrEmpty();

        Regex.IsMatch(input, pattern).Should().BeTrue();
        mockLogger.Verify(x => x.LogValidation(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    #endregion

    #region Null and Empty Assertions

    [Fact]
    public void StringValues_NullAndEmptyAssertions_ShouldValidateCorrectly()
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        string? nullString = null;
        string emptyString = string.Empty;
        string whitespaceString = "   ";
        string validString = "Valid Content";

        // Act & Assert
        nullString.Should().BeNull()
            .And.NotBe(string.Empty, "null is different from empty");

        emptyString.Should().BeEmpty()
            .And.NotBeNull()
            .And.HaveLength(0);

        whitespaceString.Should()
            .NotBeNullOrEmpty()
            .And.BeNullOrWhiteSpace()
            .And.HaveLength(3);

        validString.Should()
            .NotBeNullOrWhiteSpace()
            .And.StartWith("Valid")
            .And.EndWith("Content");

        mockLogger.Object.Should().BeAssignableTo<IValidationLogger>();
    }

    [Fact]
    public void NullableValues_ShouldAssertCorrectly()
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        int? nullableWithValue = 42;
        int? nullableWithoutValue = null;

        // Act & Assert
        nullableWithValue.Should()
            .HaveValue()
            .And.Be(42);

        nullableWithoutValue.Should()
            .NotHaveValue()
            .And.BeNull();

        mockLogger.Object.Should().NotBeNull();
    }

    #endregion

    #region Numeric Edge Cases

    [Theory]
    [TheoryData<int, string>]
    public class NumericEdgeCasesData : TheoryData<int, string>
    {
        public NumericEdgeCasesData()
        {
            Add(int.MaxValue, "Maximum integer value");
            Add(int.MinValue, "Minimum integer value");
            Add(0, "Zero value");
            Add(-1, "Negative value");
        }
    }

    [Theory]
    [ClassData(typeof(NumericEdgeCasesData))]
    public void IntegerValues_EdgeCases_ShouldHandleCorrectly(int value, string description)
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();

        // Act & Assert
        value.Should()
            .BeInRange(int.MinValue, int.MaxValue, because: "all integers are valid")
            .And.BeOfType<int>();

        if (value == int.MaxValue)
        {
            value.Should().Be(2147483647)
                .And.BePositive()
                .And.BeGreaterThan(0);
        }
        else if (value == int.MinValue)
        {
            value.Should().Be(-2147483648)
                .And.BeNegative()
                .And.BeLessThan(0);
        }

        mockLogger.Object.Should().NotBeNull();
    }

    [Fact]
    public void DoubleValues_SpecialCases_ShouldValidateCorrectly()
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        var service = new ValidationService(mockLogger.Object);
        double nanValue = double.NaN;
        double positiveInfinity = double.PositiveInfinity;
        double negativeInfinity = double.NegativeInfinity;
        double normalValue = 123.456;

        // Act & Assert
        nanValue.Should()
            .Be(double.NaN)
            .And.NotBe(0)
            .And.Match(d => double.IsNaN(d));

        positiveInfinity.Should()
            .Be(double.PositiveInfinity)
            .And.BePositive()
            .And.Match(d => double.IsPositiveInfinity(d));

        negativeInfinity.Should()
            .Be(double.NegativeInfinity)
            .And.BeNegative()
            .And.Match(d => double.IsNegativeInfinity(d));

        normalValue.Should()
            .BeApproximately(123.46, 0.01)
            .And.BeInRange(100, 200)
            .And.BeGreaterThan(100)
            .And.BeLessThan(200);

        // Test with service
        var result = service.CalculateRiskScore(normalValue);
        result.Should().BeApproximately(185.184, 0.01);
        
        mockLogger.Verify(x => x.LogValidation("RiskScore", true), Times.Once);
    }

    #endregion

    #region Collection Assertions

    [Fact]
    public void Collections_ContainmentAndOrder_ShouldValidateCorrectly()
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var duplicates = new[] { 1, 2, 2, 3 };
        var unordered = new[] { 5, 1, 3, 2, 4 };

        // Act & Assert
        numbers.Should()
            .HaveCount(5)
            .And.Contain(3)
            .And.NotContain(10)
            .And.ContainInOrder(1, 2, 3)
            .And.BeInAscendingOrder()
            .And.OnlyHaveUniqueItems()
            .And.StartWith(1)
            .And.EndWith(5);

        duplicates.Should()
            .HaveCount(4)
            .And.Contain(2)
            .And.NotOnlyHaveUniqueItems()
            .And.ContainSingle(x => x == 3);

        unordered.Should()
            .HaveCount(5)
            .And.BeEquivalentTo(numbers, "they contain the same elements")
            .And.NotBeInAscendingOrder();

        mockLogger.Object.Should().BeAssignableTo<IValidationLogger>();
    }

    [Fact]
    public void Collections_ComplexObjects_ShouldAssertEquivalence()
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        
        var expected = new List<User>
        {
            new User { Id = 1, Name = "Alice", Email = "alice@test.com" },
            new User { Id = 2, Name = "Bob", Email = "bob@test.com" }
        };

        var actual = new List<User>
        {
            new User { Id = 2, Name = "Bob", Email = "bob@test.com" },
            new User { Id = 1, Name = "Alice", Email = "alice@test.com" }
        };

        // Act & Assert
        actual.Should()
            .BeEquivalentTo(expected, options => options.WithoutStrictOrdering())
            .And.HaveCount(2)
            .And.AllSatisfy(user =>
            {
                user.Id.Should().BePositive();
                user.Name.Should().NotBeNullOrEmpty();
                user.Email.Should().MatchRegex(@"^\S+@\S+\.\S+$");
            });

        actual.Should().Contain(u => u.Name == "Alice")
            .And.Contain(u => u.Email.Contains("bob"));

        mockLogger.Verify(x => x.LogValidation(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    #endregion

    #region Type Assertions

    [Fact]
    public void TypeChecks_ShouldValidateCorrectly()
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        var service = new ValidationService(mockLogger.Object);
        object stringObject = "Hello";
        object intObject = 42;
        IValidationLogger logger = mockLogger.Object;

        // Act & Assert
        service.Should()
            .BeOfType<ValidationService>()
            .And.NotBeNull();

        stringObject.Should()
            .BeOfType<string>()
            .And.BeAssignableTo<object>()
            .And.NotBeOfType<int>();

        intObject.Should()
            .BeOfType<int>()
            .And.BeAssignableTo<IComparable>()
            .And.NotBeOfType<string>();

        logger.Should()
            .BeAssignableTo<IValidationLogger>()
            .And.NotBeNull();

        mockLogger.Object.Should()
            .Implement<IValidationLogger>()
            .And.BeAssignableTo<IValidationLogger>();
    }

    [Theory]
    [InlineData(typeof(string), typeof(object))]
    [InlineData(typeof(int), typeof(ValueType))]
    [InlineData(typeof(List<int>), typeof(IEnumerable<int>))]
    public void TypeHierarchy_ShouldValidateInheritance(Type derivedType, Type baseType)
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        var instance = Activator.CreateInstance(derivedType);

        // Act & Assert
        instance.Should()
            .NotBeNull()
            .And.BeAssignableTo(baseType);

        derivedType.Should()
            .BeAssignableTo(baseType)
            .And.NotBeNull();

        mockLogger.Object.Should().NotBeNull();
    }

    #endregion

    #region Date/Time Assertions

    [Fact]
    public void DateTimeValues_ShouldValidateTemporalRelationships()
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();
        var now = DateTime.Now;
        var yesterday = now.AddDays(-1);
        var tomorrow = now.AddDays(1);
        var almostNow = now.AddMilliseconds(100);

        // Act & Assert
        now.Should()
            .BeAfter(yesterday)
            .And.BeBefore(tomorrow)
            .And.BeCloseTo(almostNow, TimeSpan.FromSeconds(1))
            .And.BeSameDateAs(DateTime.Today);

        yesterday.Should()
            .BeBefore(now)
            .And.NotBeSameDateAs(tomorrow)
            .And.HaveYear(now.Year);

        tomorrow.Should()
            .BeAfter(now)
            .And.BeOnOrAfter(now)
            .And.NotBeCloseTo(yesterday, TimeSpan.FromHours(1));

        mockLogger.Object.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(DateRangeData))]
    public void DateTimeRanges_ShouldValidateCorrectly(DateTime date, DateTime rangeStart, DateTime rangeEnd, bool shouldBeInRange)
    {
        // Arrange
        var mockLogger = new Mock<IValidationLogger>();

        // Act
        var isInRange = date >= rangeStart && date <= rangeEnd;

        // Assert
        isInRange.Should().Be(shouldBeInRange);

        if (shouldBeInRange)
        {
            date.Should()
                .BeOnOrAfter(rangeStart)
                .And.BeOnOrBefore(rangeEnd);
        }
        else
        {
            date.Should()
                .Match<DateTime>(d => d < rangeStart || d > rangeEnd);
        }

        mockLogger.Verify(x => x.LogValidation(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    public static IEnumerable<object[]> DateRangeData()
    {
        var baseDate = new DateTime(2024, 10, 27);
        yield return new object[] { baseDate, baseDate.AddDays(-5), baseDate.AddDays(5), true };
        yield return new object[] { baseDate.AddDays(10), baseDate, baseDate.AddDays(5), false };
        yield return new object[] { baseDate.AddDays(-10), baseDate, baseDate.AddDays(5), false };
    }

    #endregion

    #region ClassData Implementation

    public class InvalidPasswordTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "short", "too short (less than 8 characters)" };
            yield return new object[] { "nouppercase123!", "missing uppercase letter" };
            yield return new object[] { "NOLOWERCASE123!", "missing lowercase letter" };
            yield return new object[] { "NoDigits!", "missing digit" };
            yield return new object[] { "NoSpecialChar123", "missing special character" };
            yield return new object[] { "", "empty string" };
            yield return new object[] { null!, "null value" };
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion
}