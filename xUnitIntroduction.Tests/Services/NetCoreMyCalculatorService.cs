using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitIntroduction.Services;



  namespace xUnitIntroduction.Tests.Services
  {
    public class MyCalculatorServiceFixture : IDisposable
    {
      public ServiceProvider ServiceProvider { get; private set; }

      public MyCalculatorServiceFixture()
      {
        var services = new ServiceCollection();

        services.AddScoped<IMyCalculator, MyCalculator>();
        services.AddScoped<MyCalculatorService>();

        ServiceProvider = services.BuildServiceProvider();
      }

      public void Dispose()
      {
        ServiceProvider?.Dispose();
      }
    }

    public class MyCalculatorServiceWithFixtureTests : IClassFixture<MyCalculatorServiceFixture>
    {
      private readonly MyCalculatorServiceFixture _fixture;

      public MyCalculatorServiceWithFixtureTests(MyCalculatorServiceFixture fixture)
      {
        _fixture = fixture;
      }

      [Fact]
      public void Add_ShouldReturnCorrectSum_WhenGivenTwoNumbers()
      {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();
        var calculatorService = scope.ServiceProvider.GetRequiredService<MyCalculatorService>();

        // Act
        var result = calculatorService.Add(10, 5);

        // Assert
        Assert.Equal(15, result);
      }

      [Fact]
      public void Substract_ShouldReturnCorrectDifference_WhenGivenTwoNumbers()
      {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();
        var calculatorService = scope.ServiceProvider.GetRequiredService<MyCalculatorService>();

        // Act
        var result = calculatorService.Substract(10, 4);

        // Assert
        Assert.Equal(6, result);
      }

      [Theory]
      [InlineData(10, 5, 15)]
      [InlineData(0, 0, 0)]
      [InlineData(-5, 5, 0)]
      [InlineData(100, 200, 300)]
      public void Add_ShouldReturnCorrectSum_ForVariousInputs(double a, double b, double expected)
      {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();
        var calculatorService = scope.ServiceProvider.GetRequiredService<MyCalculatorService>();

        // Act
        var result = calculatorService.Add(a, b);

        // Assert
        Assert.Equal(expected, result);
      }

      [Theory]
      [InlineData(10, 4, 6)]

      public void Substract_ShouldReturnCorrectDifference_ForVariousInputs(double a, double b, double expected)
      {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();
        var calculatorService = scope.ServiceProvider.GetRequiredService<MyCalculatorService>();

        // Act
        var result = calculatorService.Substract(a, b);

        // Assert
        Assert.Equal(expected, result);
      }
    }
  }

