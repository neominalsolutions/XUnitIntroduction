using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitIntroduction.Services;

namespace xUnitIntroduction.Tests.Services
{

 


  public class CalculatorServiceTest
  {
    private readonly CalculatorService calculatorService;

    public CalculatorServiceTest()
    {
      // fixture
      calculatorService = new CalculatorService();
    }

    
    [Fact(DisplayName = "ShouldReturnSum_WhenAddMethod -> iki değerin toplamı pozitif değer döndürmelidir.")] // dışarıdan parametresiz çalış
    public void ShouldReturnSum_WhenAddMethod()
    {
      // aRRANGEMENT AŞAMASI yukarıda yaptık
      // act
      double actualValue = calculatorService.Add(2.0, 5.0);

      // Assert
      Assert.Equal(7.0, actualValue);
      Assert.True(actualValue > 0); 

    }

    [Theory] // parametreli unitesteler theory olarak tanımlanır
    [InlineData(3.0, 5.0, 8.0)]
    [InlineData(1.0,2.0,3.0)] // dışarıdan parametresiz çalış
    public void ShouldReturnSum_WhenAddMethod_WithParams(double a, double b, double expectedValue)
    {
      // aRRANGEMENT AŞAMASI yukarıda yaptık
      // act
      double actualValue = calculatorService.Add(a, b);

      // Assert
      Assert.True(expectedValue == actualValue);
     

    }

    // 2 durum Edge Test kategorisine girer.

    [Fact]
    public void Should_ThrowDivedByZeroException_WhenDivideByZero()
    {

      Assert.Throws<DivideByZeroException>(() => calculatorService.Divide(5.0, 0));

    }

    [Fact]
    public void Should_Multiply_WhenDoubleMaxValue()
    {
      double a = Double.MaxValue;

      Assert.Throws<OverflowException>(() => calculatorService.Multiply(a, 5));

    }


    [Fact]
    public void Should_BeNegative_WhenSubstract()
    {
      double a = Double.MaxValue;
      Stopwatch sp = Stopwatch.StartNew();

      var actualValue = calculatorService.Substract(-5, 10);
      sp.Stop();

      // Bu aslında hızlıca sonuç döndürmesi gereken bir işlem iken uzun sürdüğünden test başarızı oluyor.

      // Not: Bunun gibi 100 lerce servisin direk olarak beklediğini düşünürken uygulama canlıya alınırken bütün test methodları saatlerce bizi bekletecek. çözüm mocklama işlemleri.
      Assert.False(sp.Elapsed.TotalMilliseconds > 100);

    }



    // Not: Sonucun başarılı bir dönüş değeri beklentisi olup testen kalıyorsak bu durumda kodda birşeyler eksiktir. Bu Exception durumunu koda tanımlamalıyız.


    // Edge Test ->  Double.Max ile Edge Test
    // Divided By Zero Edge Test
  }
}
