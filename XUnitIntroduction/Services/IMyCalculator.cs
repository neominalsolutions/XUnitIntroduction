namespace XUnitIntroduction.Services
{
  // Mock için kullanacağımız interface
  public interface IMyCalculator
  {
    double Add(double a, double b);
    double Substract(double a, double b);
    double Multiply(double a, double b);
    double Divide(double a, double b);
  }
}
