namespace XUnitIntroduction.Services
{
  // low level class
  // STUB
  public class MyCalculator : IMyCalculator
  {
    // moq ile çalışırken method verify adımı için virtual yada abstract üyeleri tercih etmemiz gerekir.
    public virtual double Add(double a, double b)
    {
      return a + b;
    }

    public virtual double Divide(double a, double b)
    {
      return a / b;
    }

    public virtual double Multiply(double a, double b)
    {
      return (a * b);
    }

    // Eğer mocklama işlemi yapılırken interface yerine sınıf mocklanırsa methodların virtual yada abstract olarak tanımlanması gerekir.
    public virtual  double Substract(double a, double b)
    {
      Thread.Sleep(3000);

      return a - b;
    }
  }
}
