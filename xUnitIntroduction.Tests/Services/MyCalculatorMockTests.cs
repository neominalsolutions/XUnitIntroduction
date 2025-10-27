using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

      var sw = Stopwatch.StartNew();

      // bu sayede MyCalculator real class kullanmayacaktır
      // stub
      //var mockService = new Mock<MyCalculator>();
      var mockService = new Mock<IMyCalculator>(); // interface mocklandı bu şekilde tanımlarsak, methodları abstract yada virtual yapmaya gerek kalmaz.
      // sut 
      // dependecy mocklanıp gönderiliyor
      var calculatorService = new MyCalculatorService(mockService.Object);

     

      // setup mock ile ilgili bir aşama
      mockService.Setup(x => x.Substract(It.IsAny<double>(), It.IsAny<double>())).Returns<double, double>((x, y) => x - y).Callback(() =>
      {
        // loglama, ek işlemler yapılabilir.
        Console.WriteLine("Substract methodu çağırıldı.");
      });

      // act
      var result =  calculatorService.Substract(10, 4);

      // assert
      result.Should().Be(6).And.BePositive();


      // verify
      mockService.Verify(x => x.Substract(10, 4),Times.Once);
      // bu method en az bir kez çağırıldımı ?
      sw.Stop();

      // burada da timeout süresini test ettik. elapsed time üzerinden.
      sw.Elapsed.TotalMilliseconds.Should().BeLessThan(100);


    }

  }
}
