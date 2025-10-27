namespace XUnitIntroduction.Services
{
  // SUT
  // High Level Class
  public class MyCalculatorService
  {

    // Mock Dependecy
    private readonly IMyCalculator _calculator;

    public MyCalculatorService(IMyCalculator calculator)
    {
      _calculator = calculator;
    }

    public double Add(double a, double b)
    {
      return _calculator.Add(a, b);
    }

    public double Substract(double a, double b)
    {
      return _calculator.Substract(a, b);
    }

  }
}
