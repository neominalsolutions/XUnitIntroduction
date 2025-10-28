using XUnitIntroduction.dtos;

namespace XUnitIntroduction.Application
{
  public interface IOrderSubmitApplicationService
  {
    void Handle(SubmitOrderRequest orderRequest);
  }
}
