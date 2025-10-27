using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using XUnitIntroduction.Controllers;
using XUnitIntroduction.Services;
using Xunit;
using XUnitIntroduction.dtos;

namespace xUnitIntroduction.Tests.Controllers
{
  public class CalculatorsControllerTests
  {
    private readonly Mock<IMyCalculator> _mockCalculator; // Mocked Dependency
    private readonly CalculatorsController _controller; // SUT Controller Under Test

    public CalculatorsControllerTests()
    {
      _mockCalculator = new Mock<IMyCalculator>();
      _controller = new CalculatorsController(_mockCalculator.Object);
    }

    #region Add Tests

    [Fact]
    public void Add_ShouldReturnCreated_WithCorrectResult_WhenValidRequest()
    {
      // Arrange
      var request = new AddRequest (a: 10, b: 5);
      _mockCalculator.Setup(x => x.Add(request.a, request.b)).Returns(15);

      // Act
      var result = _controller.Add(request);

      // Assert
      result.Should().BeOfType<CreatedResult>();
      var createdResult = result as CreatedResult;
      createdResult.StatusCode.Should().Be(201);
      createdResult.Value.Should().Be(15);
      createdResult.Location.Should().Be("https://localhost:7210/api/calculators/add");

      _mockCalculator.Verify(x => x.Add(request.a, request.b), Times.Once);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(-5, 5, 0)]
    [InlineData(100.5, 200.3, 300.8)]
    [InlineData(-10, -20, -30)]
    public void Add_ShouldReturnCreated_WithCorrectResult_ForVariousInputs(double a, double b, double expected)
    {
      // Arrange
      var request = new AddRequest(a,b);
      _mockCalculator.Setup(x => x.Add(a, b)).Returns(expected);

      // Act
      var result = _controller.Add(request);

      // Assert
      var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
      createdResult.Value.Should().Be(expected);
    }

    [Fact]
    public void Add_ShouldInvokeCalculatorOnce_WhenCalled()
    {
      // Arrange
      var request = new AddRequest (a: 1, b: 2);
      _mockCalculator.Setup(x => x.Add(It.IsAny<double>(), It.IsAny<double>())).Returns(3);

      // Act
      _controller.Add(request);

      // Assert
      _mockCalculator.Verify(x => x.Add(1, 2), Times.Once);
      _mockCalculator.VerifyNoOtherCalls();
    }

    #endregion

    #region Multiply Tests

    [Fact]
    public void Multiply_ShouldReturnOk_WithCorrectResult_WhenValidRequest()
    {
      // Arrange
      var request = new MultiplyRequest (a: 10, b: 5);
      _mockCalculator.Setup(x => x.Multiply(request.a, request.b)).Returns(50);

      // Act
      var result = _controller.Multiply(request);

      // Assert
      result.Should().BeOfType<OkObjectResult>();
      var okResult = result as OkObjectResult;
      okResult.StatusCode.Should().Be(200);
      okResult.Value.Should().Be(50);

      _mockCalculator.Verify(x => x.Multiply(request.a, request.b), Times.Once);
    }

    [Theory]
    [InlineData(0, 5, 0)]
    [InlineData(5, 0, 0)]
    [InlineData(-5, 5, -25)]
    [InlineData(-5, -5, 25)]
    [InlineData(2.5, 4, 10)]
    public void Multiply_ShouldReturnOk_WithCorrectResult_ForVariousInputs(double a, double b, double expected)
    {
      // Arrange
      var request = new MultiplyRequest( a,b);
      _mockCalculator.Setup(x => x.Multiply(a, b)).Returns(expected);

      // Act
      var result = _controller.Multiply(request);

      // Assert
      var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
      okResult.Value.Should().Be(expected);
    }

    [Fact]
    public void Multiply_ShouldInvokeCalculatorOnce_WhenCalled()
    {
      // Arrange
      var request = new MultiplyRequest (a: 3, b: 4);
      _mockCalculator.Setup(x => x.Multiply(It.IsAny<double>(), It.IsAny<double>())).Returns(12);

      // Act
      _controller.Multiply(request);

      // Assert
      _mockCalculator.Verify(x => x.Multiply(3, 4), Times.Once);
      _mockCalculator.VerifyNoOtherCalls();
    }

    #endregion

    #region Substract Tests

    [Fact]
    public void Substract_ShouldReturnOk_WithCorrectResult_WhenValidRequest()
    {
      // Arrange
      var request = new SubstractRequest (a: 10, b: 4);
      _mockCalculator.Setup(x => x.Substract(request.a, request.b)).Returns(6);

      // Act
      var result = _controller.Substract(request);

      // Assert
      result.Should().BeOfType<OkObjectResult>();
      var okResult = result as OkObjectResult;
      okResult.StatusCode.Should().Be(200);
      okResult.Value.Should().Be(6);

      _mockCalculator.Verify(x => x.Substract(request.a, request.b), Times.Once);
    }

    [Theory]
    [InlineData(10, 4, 6)]
    [InlineData(0, 5, -5)]
    [InlineData(-3, -2, -1)]
    [InlineData(100, 50, 50)]
    [InlineData(5.5, 2.5, 3)]
    public void Substract_ShouldReturnOk_WithCorrectResult_ForVariousInputs(double a, double b, double expected)
    {
      // Arrange
      var request = new SubstractRequest(a,b);
      _mockCalculator.Setup(x => x.Substract(a, b)).Returns(expected);

      // Act
      var result = _controller.Substract(request);

      // Assert
      var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
      okResult.Value.Should().Be(expected);
    }

    [Fact]
    public void Substract_ShouldInvokeCalculatorOnce_WhenCalled()
    {
      // Arrange
      var request = new SubstractRequest (a: 10, b: 5);
      _mockCalculator.Setup(x => x.Substract(It.IsAny<double>(), It.IsAny<double>())).Returns(5);

      // Act
      _controller.Substract(request);

      // Assert
      _mockCalculator.Verify(x => x.Substract(10, 5), Times.Once);
      _mockCalculator.VerifyNoOtherCalls();
    }

    #endregion

    #region Divide Tests

    [Fact]
    public void Divide_ShouldReturnOk_WithCorrectResult_WhenValidRequest()
    {
      // Arrange
      var request = new DivideRequest (a: 10, b: 2);
      _mockCalculator.Setup(x => x.Divide(request.a, request.b)).Returns(5);

      // Act
      var result = _controller.Divide(request);

      // Assert
      result.Should().BeOfType<OkObjectResult>();
      var okResult = result as OkObjectResult;
      okResult.StatusCode.Should().Be(200);
      okResult.Value.Should().Be(5);

      _mockCalculator.Verify(x => x.Divide(request.a, request.b), Times.Once);
    }

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(100, 4, 25)]
    [InlineData(-10, 2, -5)]
    [InlineData(-10, -2, 5)]
    [InlineData(7.5, 2.5, 3)]
    public void Divide_ShouldReturnOk_WithCorrectResult_ForVariousInputs(double a, double b, double expected)
    {
      // Arrange
      var request = new DivideRequest(a, b);
      _mockCalculator.Setup(x => x.Divide(a, b)).Returns(expected);

      // Act
      var result = _controller.Divide(request);

      // Assert
      var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
      okResult.Value.Should().Be(expected);
    }

    [Fact]
    public void Divide_ShouldThrowException_WhenDivideByZero()
    {
      // Arrange
      var request = new DivideRequest (a: 10, b: 0);
      _mockCalculator.Setup(x => x.Divide(request.a, request.b))
                     .Throws<DivideByZeroException>();

      // Act
      // Not:  Fluent Assertions kullanarak exception testi yaparken Action delegate kullanýlýr.
      // try catch yerine kullanýlacak yöntem.
      Action act = () => _controller.Divide(request);

      // Assert
      act.Should().Throw<DivideByZeroException>();
      _mockCalculator.Verify(x => x.Divide(10, 0), Times.Once);
    }

    [Fact]
    public void Divide_ShouldInvokeCalculatorOnce_WhenCalled()
    {
      // Arrange
      var request = new DivideRequest (a: 20, b: 4);
      _mockCalculator.Setup(x => x.Divide(It.IsAny<double>(), It.IsAny<double>())).Returns(5);

      // Act
      _controller.Divide(request);

      // Assert
      _mockCalculator.Verify(x => x.Divide(20, 4), Times.Once);
      _mockCalculator.VerifyNoOtherCalls();
    }

    #endregion

    #region Edge Cases and Error Handling

    [Fact]
    public void Add_ShouldHandleLargeNumbers()
    {
      // Arrange
      var request = new AddRequest( a : double.MaxValue, b : 0 );
      _mockCalculator.Setup(x => x.Add(request.a, request.b)).Returns(double.MaxValue);

      // Act
      var result = _controller.Add(request);

      // Assert
      var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
      createdResult.Value.Should().Be(double.MaxValue);
    }

    [Fact]
    public void Multiply_ShouldHandleDecimalPrecision()
    {
      // Arrange
      var request = new MultiplyRequest(0.1,0.2)
;
      _mockCalculator.Setup(x => x.Multiply(request.a, request.b)).Returns(0.02);

      // Act
      var result = _controller.Multiply(request);

      // Assert
      var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
      okResult.Value.Should().Be(0.02);
    }

    [Fact]
    public void Substract_ShouldHandleNegativeResults()
    {
      // Arrange
      var request = new SubstractRequest (a: 5, b: 10);
      _mockCalculator.Setup(x => x.Substract(request.a, request.b)).Returns(-5);

      // Act
      var result = _controller.Substract(request);

      // Assert
      var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
      okResult.Value.Should().Be(-5);
    }

    [Fact]
    public void Divide_ShouldReturnInfinity_WhenDividingByZero_AndCalculatorReturnsInfinity()
    {
      // Arrange
      var request = new DivideRequest( a: 10, b: 0 );
      _mockCalculator.Setup(x => x.Divide(request.a, request.b)).Returns(double.NegativeInfinity);

      // Act
      var result = _controller.Divide(request);

      // Assert
      var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
      okResult.Value.Should().Be(double.NegativeInfinity);
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldInitializeController_WithDependency()
    {
      // Arrange & Act
      var controller = new CalculatorsController(_mockCalculator.Object);

      // Assert
      controller.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenDependencyIsNull()
    {
      // Arrange & Act
      Action act = () => new CalculatorsController(null);

      // var calc = new CalculatorsController(null);
      //Action act2 = () =>   calc.Add(new AddRequest(1,2));


      // Assert
      act.Should().Throw<ArgumentNullException>();
    }

    #endregion
  }
}