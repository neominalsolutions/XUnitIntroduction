using FluentAssertions;
using Moq;
using Xunit;
using XUnitIntroduction.Application;
using XUnitIntroduction.dtos;
using XUnitIntroduction.Entities;

namespace XUnitIntroduction.Tests.Application
{
  public class SubmitOrderApplicationServiceTests
  {
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly SubmitOrderApplicationService _sut;

    public SubmitOrderApplicationServiceTests()
    {
      _orderRepositoryMock = new Mock<IOrderRepository>();
      _emailSenderMock = new Mock<IEmailSender>();
      _sut = new SubmitOrderApplicationService(_orderRepositoryMock.Object, _emailSenderMock.Object);
    }

    [Fact]
    public void Handle_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
      // Arrange
      SubmitOrderRequest request = null;

      // Act
      Action act = () => _sut.Handle(request);

      // Assert
      act.Should().Throw<ArgumentNullException>()
        .WithParameterName("orderRequest");
    }

    [Fact]
    public void Handle_ShouldThrowArgumentNullException_WhenCodeIsNull()
    {
      // Arrange
      var request = new SubmitOrderRequest(null);

      // Act
      Action act = () => _sut.Handle(request);

      // Assert
      act.Should().Throw<ArgumentNullException>()
        .WithParameterName("code");
    }

    [Fact]
    public void Handle_ShouldThrowArgumentNullException_WhenCodeIsEmpty()
    {
      // Arrange
      var request = new SubmitOrderRequest(string.Empty);

      // Act
      Action act = () => _sut.Handle(request);

      // Assert
      act.Should().Throw<ArgumentNullException>()
        .WithParameterName("code");
    }

    [Fact]
    public void Handle_ShouldThrowException_WhenCodeIsLessThan10Characters()
    {
      // Arrange
      var request = new SubmitOrderRequest("ORD123");

      // Act
      Action act = () => _sut.Handle(request);

      // Assert
      act.Should().Throw<Exception>()
        .WithMessage("Code is not Valid");
      
      _orderRepositoryMock.Verify(x => x.Save(It.IsAny<Order>()), Times.Never);
      _emailSenderMock.Verify(x => x.SendEmail(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Handle_ShouldThrowException_WhenCodeDoesNotStartWithORD()
    {
      // Arrange
      var request = new SubmitOrderRequest("ABC1234567");

      // Act
      Action act = () => _sut.Handle(request);

      // Assert
      act.Should().Throw<Exception>()
        .WithMessage("Code is not Valid");
      
      _orderRepositoryMock.Verify(x => x.Save(It.IsAny<Order>()), Times.Never);
      _emailSenderMock.Verify(x => x.SendEmail(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Handle_ShouldThrowException_WhenCodeStartsWithORDButLessThan10Characters()
    {
      // Arrange
      var request = new SubmitOrderRequest("ORD12");

      // Act
      Action act = () => _sut.Handle(request);

      // Assert
      act.Should().Throw<Exception>()
        .WithMessage("Code is not Valid");
      
      _orderRepositoryMock.Verify(x => x.Save(It.IsAny<Order>()), Times.Never);
      _emailSenderMock.Verify(x => x.SendEmail(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Handle_ShouldSaveAndSendEmail_WhenCodeIsExactly10CharactersAndValid()
    {
      // Arrange
      var request = new SubmitOrderRequest("ORD1234567");

      // Act
      _sut.Handle(request);

      // Assert
      _orderRepositoryMock.Verify(x => x.Save(It.Is<Order>(o => o.Code == "ORD1234567")), Times.Once);
      _emailSenderMock.Verify(x => x.SendEmail("Kayýt baþarýlý"), Times.Once);
    }

    [Fact]
    public void Handle_ShouldSaveAndSendEmail_WhenOrderIsValid()
    {
      // Arrange
      var request = new SubmitOrderRequest("ORD12345678910");

      // Act
      _sut.Handle(request);

      // Assert
      _orderRepositoryMock.Verify(x => x.Save(It.Is<Order>(o => o.Code == "ORD12345678910")), Times.Once);
      _emailSenderMock.Verify(x => x.SendEmail("Kayýt baþarýlý"), Times.Once);
    }

    [Fact]
    public void Handle_ShouldCallSaveExactlyOnce_WhenOrderIsValid()
    {
      // Arrange
      var request = new SubmitOrderRequest("ORD1234567890");

      // Act
      _sut.Handle(request);

      // Assert
      _orderRepositoryMock.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public void Handle_ShouldCallSendEmailExactlyOnce_WhenOrderIsValid()
    {
      // Arrange
      var request = new SubmitOrderRequest("ORD1234567890");

      // Act
      _sut.Handle(request);

      // Assert
      _emailSenderMock.Verify(x => x.SendEmail("Kayýt baþarýlý"), Times.Once);
    }

    [Theory]
    [InlineData("ORD1234567")] // Exactly 10 characters
    [InlineData("ORD12345678")] // 11 characters
    [InlineData("ORD123456789012345")] // Long code
    public void Handle_ShouldSaveAndSendEmail_WhenCodeIsValidWithVariousLengths(string code)
    {
      // Arrange
      var request = new SubmitOrderRequest(code);

      // Act
      _sut.Handle(request);

      // Assert
      _orderRepositoryMock.Verify(x => x.Save(It.Is<Order>(o => o.Code == code)), Times.Once);
      _emailSenderMock.Verify(x => x.SendEmail("Kayýt baþarýlý"), Times.Once);
    }
  }
}