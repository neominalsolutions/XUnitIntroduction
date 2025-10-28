using XUnitIntroduction.Entities;

namespace XUnitIntroduction.Application
{
  public class OrderRepository : IOrderRepository
  {
    public void Save(Order order)
    {
      Console.WriteLine($"Order with code {order.Code} saved to the database.");
    }
  }
}
