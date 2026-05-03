using FCG.Reviews.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Reviews.API.Common.Outputs;

public static class ResultExtensions
{
    public static IActionResult ToCreatedActionResult<T>(this ResultData<T> result, string path = "")
    {
        if (!result.IsSuccess)
            return new BadRequestObjectResult(result);

        return new CreatedResult(path, result);
    }

    public static IActionResult ToOkActionResult<T>(this ResultData<T> result)
    {
        if (!result.IsSuccess)
            return new BadRequestObjectResult(result);

        return new OkObjectResult(result);
    }
}
