namespace XUnitIntroduction.Services
{




  public class CalculatorService
  {
    public double Add(double a, double b) { return a + b; }
    public double Substract(double a, double b) {

      Thread.Sleep(3000); // api call işlem uzun sürme 3sniye bekleme
      
      return a - b; 
    
    }

    public double Multiply(double a, double b) {
       
      if(a == Double.MaxValue ||  b == Double.MaxValue)
      {
        throw new OverflowException();
      }


      return a * b;}

    public double Divide(double a, double b) {

      if (b == 0)
        throw new DivideByZeroException();

      return a / b;
      
    }
  }
}
