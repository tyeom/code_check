using HandlingExceptions.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HandlingExceptions.Controllers;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/Error2")]
    public IActionResult Error2()
    {
        var exceptionDetails = HttpContext.Features.Get<ExceptionDetails>();

        return StatusCode(StatusCodes.Status500InternalServerError, exceptionDetails);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/Error")]
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context.Error;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "요청 처리중 오류가 발생 했습니다.",
            Detail = exception.Message
        };

        return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
    }
}
