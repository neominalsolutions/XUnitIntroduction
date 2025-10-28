using XUnitIntroduction.dtos;
using XUnitIntroduction.Entities;

namespace XUnitIntroduction.Application
{
  // sut
  public class SubmitOrderApplicationService : IOrderSubmitApplicationService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailSender _emailSender; // mock dependecy

    public SubmitOrderApplicationService(IOrderRepository orderRepository, IEmailSender emailSender )
    {
      _orderRepository = orderRepository;
      _emailSender = emailSender;
    }

    public void Handle(SubmitOrderRequest orderRequest)
    {
      if (orderRequest == null)
      {
        throw new ArgumentNullException(nameof(orderRequest));
      }

      if (string.IsNullOrEmpty(orderRequest.code))
      {
        throw new ArgumentNullException(nameof(orderRequest.code));
      }

      if (orderRequest.code.Length < 10 || !orderRequest.code.StartsWith("ORD"))
      {
        throw new Exception("Code is not Valid");
      }

      var entity = new Order();
      entity.Code = orderRequest.code;

      _orderRepository.Save(entity);
      _emailSender.SendEmail("Kayıt başarılı");
    }
  }
}
