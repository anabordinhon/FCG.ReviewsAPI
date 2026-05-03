using FCG.Reviews.API.Common.Outputs;
using FCG.Reviews.Application.Reviews.UseCases.Commands.AddReview;
using FCG.Reviews.Application.Reviews.UseCases.Queries.GetReviewsByGame;
using FCG.Reviews.Domain.Common.Ports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Reviews.API.Controllers.Reviews;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly IUserContext _userContext;
    public ReviewsController(IUserContext userContext)
    {
        _userContext = userContext;
    }

    [HttpPost]
    public async Task<IActionResult> AddReview(
        [FromBody] AddReviewInput input,
        [FromServices] IAddReviewCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userContext.GetCurrentUserId();

            var command = input.MapToCommand(userId);
            var result = await handler.Handle(command, cancellationToken);

            if (!result.IsSuccess)
                return result.ToOkActionResult();

            return result.ToCreatedActionResult($"/api/reviews/game/{input.GameId}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }

    }

    [HttpGet("game/{gameId}")]
    public async Task<IActionResult> GetByGame(
        [FromRoute] Guid gameId,
        [FromServices] IGetReviewsByGameQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetReviewsByGameQuery(gameId);
        var result = await handler.Handle(query, cancellationToken);
        return result.ToOkActionResult();
    }
}
