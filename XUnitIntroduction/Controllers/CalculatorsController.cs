using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XUnitIntroduction.dtos;
using XUnitIntroduction.Services;

namespace XUnitIntroduction.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CalculatorsController : ControllerBase
  {
    private readonly IMyCalculator _calculator;

    public CalculatorsController(IMyCalculator calculator)
    {
      _calculator = calculator ?? throw new ArgumentNullException();
      
    }

    [HttpPost("add")]
    public IActionResult Add([FromBody] AddRequest request)
    {
      var result = _calculator.Add(request.a, request.b);
      return Created(new Uri($"https://localhost:7210/api/calculators/add"),result);
    }

    [HttpPost("multiply")]
    public IActionResult Multiply([FromBody] MultiplyRequest request)
    {
      var result = _calculator.Multiply(request.a, request.b);
      return Ok(result);
    }

    [HttpPost("substract")]
    public IActionResult Substract([FromBody] SubstractRequest request)
    {
      var result = _calculator.Substract(request.a, request.b);
      return Ok(result);
    }

    [HttpPost("divide")]
    public IActionResult Divide([FromBody] DivideRequest request)
    {
      var result = _calculator.Divide(request.a, request.b);
      return Ok(result);
    }
  }
}
