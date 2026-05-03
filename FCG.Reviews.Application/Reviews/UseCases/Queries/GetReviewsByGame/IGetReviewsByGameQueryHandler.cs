using FCG.Reviews.Application.Common;
using FCG.Reviews.Application.Reviews.Outputs;

namespace FCG.Reviews.Application.Reviews.UseCases.Queries.GetReviewsByGame;

public interface IGetReviewsByGameQueryHandler
{
    Task<ResultData<GameReviewSummaryOutput>> Handle(GetReviewsByGameQuery query, CancellationToken cancellationToken);
}
