using FCG.Reviews.Application.Common;
using FCG.Reviews.Application.Reviews.Outputs;

namespace FCG.Reviews.Application.Reviews.UseCases.Commands.AddReview;

public interface IAddReviewCommandHandler
{
    Task<ResultData<ReviewOutput>> Handle(AddReviewCommand command, CancellationToken cancellationToken);
}
