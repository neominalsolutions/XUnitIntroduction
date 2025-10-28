using XUnitIntroduction.Entities;

namespace XUnitIntroduction.Application
{
  public interface IOrderRepository
  {
    void Save(Order order);
  }
}
