using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using XUnitIntroduction.Controllers;
using XUnitIntroduction.Services;
using XUnitIntroduction.dtos;
using Xunit;

namespace xUnitIntroduction.Tests.Controllers
{
  // Tests for XUnitIntroduction.Controllers.CalculatorsController
  // Uses Moq + FluentAssertions, follows AAA pattern
  public class CalculatorServiceTestV2
  {
    private readonly Mock<IMyCalculator> _calculatorMock; // dependency
    private readonly CalculatorsController _controller;    // SUT

    public CalculatorServiceTestV2()
    {
      // Arrange common
      _calculatorMock = new Mock<IMyCalculator>();
      _controller = new CalculatorsController(_calculatorMock.Object);
    }

    #region Add

    [Fact]
    public void Add_ShouldReturnCreated_WithCorrectLocationAndValue_WhenValidRequest()
    {
      // Arrange
      var request = new AddRequest(a: 10, b: 5);
      _calculatorMock.Setup(x => x.Add(request.a, request.b)).Returns(15);

      // Act
      var result = _controller.Add(request);

      // Assert
      var created = result.Should().BeOfType<CreatedResult>().Subject;
      created.StatusCode.Should().Be(201);
      created.Value.Should().Be(15);
      created.Location.Should().Be("https://localhost:7210/api/calculators/add");

      _calculatorMock.Verify(x => x.Add(10, 5), Times.Once);
      _calculatorMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(-5, 5, 0)]
    [InlineData(100.5, 200.3, 300.8)]
    [InlineData(-10, -20, -30)]
    public void Add_ShouldReturnCreated_WithExpectedResult_ForVariousInputs(double a, double b, double expected)
    {
      // Arrange
      var request = new AddRequest(a, b);
      _calculatorMock.Setup(x => x.Add(a, b)).Returns(expected);

      // Act
      var result = _controller.Add(request);

      // Assert
      var created = result.Should().BeOfType<CreatedResult>().Subject;
      created.Value.Should().Be(expected);
      _calculatorMock.Verify(x => x.Add(a, b), Times.Once);
    }

    #endregion

    #region Multiply

    [Fact]
    public void Multiply_ShouldReturnOk_WithCorrectValue_WhenValidRequest()
    {
      // Arrange
      var request = new MultiplyRequest(a: 3, b: 4);
      _calculatorMock.Setup(x => x.Multiply(request.a, request.b)).Returns(12);

      // Act
      var result = _controller.Multiply(request);

      // Assert
      var ok = result.Should().BeOfType<OkObjectResult>().Subject;
      ok.StatusCode.Should().Be(200);
      ok.Value.Should().Be(12);

      _calculatorMock.Verify(x => x.Multiply(3, 4), Times.Once);
      _calculatorMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(0, 5, 0)]
    [InlineData(5, 0, 0)]
    [InlineData(-5, 5, -25)]
    [InlineData(-5, -5, 25)]
    [InlineData(2.5, 4, 10)]
    public void Multiply_ShouldReturnOk_WithExpectedResult_ForVariousInputs(double a, double b, double expected)
    {
      // Arrange
      var request = new MultiplyRequest(a, b);
      _calculatorMock.Setup(x => x.Multiply(a, b)).Returns(expected);

      // Act
      var result = _controller.Multiply(request);

      // Assert
      var ok = result.Should().BeOfType<OkObjectResult>().Subject;
      ok.Value.Should().Be(expected);
      _calculatorMock.Verify(x => x.Multiply(a, b), Times.Once);
    }

    #endregion

    #region Substract

    [Fact]
    public void Substract_ShouldReturnOk_WithCorrectValue_WhenValidRequest()
    {
      // Arrange
      var request = new SubstractRequest(a: 10, b: 4);
      _calculatorMock.Setup(x => x.Substract(request.a, request.b)).Returns(6);

      // Act
      var result = _controller.Substract(request);

      // Assert
      var ok = result.Should().BeOfType<OkObjectResult>().Subject;
      ok.StatusCode.Should().Be(200);
      ok.Value.Should().Be(6);

      _calculatorMock.Verify(x => x.Substract(10, 4), Times.Once);
      _calculatorMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(10, 4, 6)]
    [InlineData(0, 5, -5)]
    [InlineData(-3, -2, -1)]
    [InlineData(100, 50, 50)]
    [InlineData(5.5, 2.5, 3)]
    public void Substract_ShouldReturnOk_WithExpectedResult_ForVariousInputs(double a, double b, double expected)
    {
      // Arrange
      var request = new SubstractRequest(a, b);
      _calculatorMock.Setup(x => x.Substract(a, b)).Returns(expected);

      // Act
      var result = _controller.Substract(request);

      // Assert
      var ok = result.Should().BeOfType<OkObjectResult>().Subject;
      ok.Value.Should().Be(expected);
      _calculatorMock.Verify(x => x.Substract(a, b), Times.Once);
    }

    #endregion

    #region Divide

    [Fact]
    public void Divide_ShouldReturnOk_WithCorrectValue_WhenValidRequest()
    {
      // Arrange
      var request = new DivideRequest(a: 20, b: 5);
      _calculatorMock.Setup(x => x.Divide(request.a, request.b)).Returns(4);

      // Act
      var result = _controller.Divide(request);

      // Assert
      var ok = result.Should().BeOfType<OkObjectResult>().Subject;
      ok.StatusCode.Should().Be(200);
      ok.Value.Should().Be(4);

      _calculatorMock.Verify(x => x.Divide(20, 5), Times.Once);
      _calculatorMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(100, 4, 25)]
    [InlineData(-10, 2, -5)]
    [InlineData(-10, -2, 5)]
    [InlineData(7.5, 2.5, 3)]
    public void Divide_ShouldReturnOk_WithExpectedResult_ForVariousInputs(double a, double b, double expected)
    {
      // Arrange
      var request = new DivideRequest(a, b);
      _calculatorMock.Setup(x => x.Divide(a, b)).Returns(expected);

      // Act
      var result = _controller.Divide(request);

      // Assert
      var ok = result.Should().BeOfType<OkObjectResult>().Subject;
      ok.Value.Should().Be(expected);
      _calculatorMock.Verify(x => x.Divide(a, b), Times.Once);
    }

    [Fact]
    public void Divide_ShouldPropagateDivideByZeroException_WhenDependencyThrows()
    {
      // Arrange
      var request = new DivideRequest(a: 10, b: 0);
      _calculatorMock
        .Setup(x => x.Divide(request.a, request.b))
        .Throws<DivideByZeroException>();

      // Act
      Action act = () => _controller.Divide(request);

      // Assert
      act.Should().Throw<DivideByZeroException>();
      _calculatorMock.Verify(x => x.Divide(10, 0), Times.Once);
    }

    [Fact]
    public void Divide_ShouldReturnInfinity_WhenDependencyReturnsInfinity()
    {
      // Arrange
      var request = new DivideRequest(a: 10, b: 0);
      _calculatorMock.Setup(x => x.Divide(request.a, request.b)).Returns(double.PositiveInfinity);

      // Act
      var result = _controller.Divide(request);

      // Assert
      var ok = result.Should().BeOfType<OkObjectResult>().Subject;
      ok.Value.Should().Be(double.PositiveInfinity);
      _calculatorMock.Verify(x => x.Divide(10, 0), Times.Once);
    }

    #endregion

    #region Constructor

    [Fact]
    public void Ctor_ShouldInitialize_WithValidDependency()
    {
      // Arrange
      // Act
      var controller = new CalculatorsController(_calculatorMock.Object);

      // Assert
      controller.Should().NotBeNull();
    }

    [Fact]
    public void Ctor_ShouldThrowArgumentNullException_WhenDependencyIsNull()
    {
      // Arrange
      // Act
      Action act = () => new CalculatorsController(null);

      // Assert
      act.Should().Throw<ArgumentNullException>();
    }

    #endregion
  }
}