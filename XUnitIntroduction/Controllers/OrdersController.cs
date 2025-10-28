using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XUnitIntroduction.Application;
using XUnitIntroduction.dtos;

namespace XUnitIntroduction.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase
  {
    private readonly IOrderSubmitApplicationService _orderSubmitApplicationService;

    public OrdersController(IOrderSubmitApplicationService orderSubmitApplicationService)
    {
      _orderSubmitApplicationService = orderSubmitApplicationService;
    }


    [HttpPost]
    public IActionResult SubmitOrder([FromBody] SubmitOrderRequest orderRequest)
    {
      _orderSubmitApplicationService.Handle(orderRequest);

      return Ok();
    }
  }
}
