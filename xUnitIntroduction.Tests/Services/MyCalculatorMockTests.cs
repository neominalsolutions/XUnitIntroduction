using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitIntroduction.Services;

namespace xUnitIntroduction.Tests.Services
{
  public class MyCalculatorMockTests
  {
    [Fact]
    public void ShouldReturnResult_WhenSubstract()
    {
      // bu sayede MyCalculator real class kullanmayacaktır
      // stub
      //var mockService = new Mock<MyCalculator>();
      var mockService = new Mock<IMyCalculator>(); // interface mocklandı bu şekilde tanımlarsak, methodları abstract yada virtual yapmaya gerek kalmaz.
      // sut 
      // dependecy mocklanıp gönderiliyor
      var calculatorService = new MyCalculatorService(mockService.Object);

      // setup mock ile ilgili bir aşama
      mockService.Setup(x => x.Substract(It.IsAny<double>(), It.IsAny<double>())).Returns<double, double>((x, y) => x - y);

      // act
      var result =  calculatorService.Substract(10, 4);

      // assert
      result.Should().Be(6).And.BePositive();


      // verify
      mockService.Verify(x => x.Substract(10, 4),Times.Once);
      // bu method en az bir kez çağırıldımı ?

    }

  }
}
