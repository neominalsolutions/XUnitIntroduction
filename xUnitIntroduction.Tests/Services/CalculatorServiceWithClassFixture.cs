using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitIntroduction.Services;

namespace xUnitIntroduction.Tests.Services
{

  public class CalculatorFixture : IDisposable
  {
    public readonly CalculatorService calculatorService;

    // Setup
    public CalculatorFixture() {
      Console.WriteLine("Setup");
     calculatorService = new CalculatorService();
    }



    public void Dispose()
    {
      // testler tamamlandı varsa temizlik yap.
      Console.WriteLine("Teardown");
    }
  }
  public class CalculatorServiceWithClassFixture:IClassFixture<CalculatorFixture>
  {

    private readonly CalculatorService _calculatorService;

    public CalculatorServiceWithClassFixture(CalculatorFixture calculatorFixture)
    {
      _calculatorService = calculatorFixture.calculatorService;
    }


      [Fact(DisplayName = "ShouldReturnSum_WhenAddMethod -> iki değerin toplamı pozitif değer döndürmelidir.")] // dışarıdan parametresiz çalış
      public void ShouldReturnSum_WhenAddMethod()
      {
        // aRRANGEMENT AŞAMASI yukarıda yaptık
        // act
        double actualValue = _calculatorService.Add(2.0, 5.0);

        // Assert
        Assert.Equal(7.0, actualValue);
        Assert.True(actualValue > 0);

      }

    [Fact(DisplayName = "ShouldReturnSum_WhenAddMethod -> iki değerin toplamı pozitif değer döndürmelidir.")] // dışarıdan parametresiz çalış
    public void ShouldReturnSum_WhenAddMethodV2()
    {
      // aRRANGEMENT AŞAMASI yukarıda yaptık
      // act
      double actualValue = _calculatorService.Add(2.0, 5.0);

      // Assert
      Assert.Equal(7.0, actualValue);
      Assert.True(actualValue > 0);

    }

  }
}
