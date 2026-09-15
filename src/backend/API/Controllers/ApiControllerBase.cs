using EnterpriseFlow.Domain.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult FromResult(Result result)
    {
        if (result.Valid)
            return Ok(result);

        return result.ResultCode switch
        {
            ResultCode.BadRequest => BadRequest(result),
            ResultCode.Unauthorized => Unauthorized(result),
            ResultCode.BusinessError => UnprocessableEntity(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result)
        };
    }
}
