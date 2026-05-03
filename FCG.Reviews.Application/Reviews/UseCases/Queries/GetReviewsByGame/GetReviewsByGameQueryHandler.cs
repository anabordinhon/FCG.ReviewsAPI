using FCG.Reviews.Application.Common;
using FCG.Reviews.Application.Reviews.Mappers;
using FCG.Reviews.Application.Reviews.Outputs;
using FCG.Reviews.Application.Reviews.Ports;

namespace FCG.Reviews.Application.Reviews.UseCases.Queries.GetReviewsByGame;

public class GetReviewsByGameQueryHandler : IGetReviewsByGameQueryHandler
{
    private readonly IReviewQueryRepository _queryRepository;

    public GetReviewsByGameQueryHandler(IReviewQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<ResultData<GameReviewSummaryOutput>> Handle(GetReviewsByGameQuery query, CancellationToken cancellationToken)
    {
        var reviews = await _queryRepository.GetByGameIdAsync(query.GameId, cancellationToken);

        if (!reviews.Any())
            return ResultData<GameReviewSummaryOutput>.Error("Nenhuma avaliação encontrada para este jogo.");

        var outputs = reviews.Select(r => r.ToOutput()).ToList();
        var average = reviews.Average(r => r.Rating);

        var summary = new GameReviewSummaryOutput(
            query.GameId,
            Math.Round(average, 2),
            reviews.Count,
            outputs
        );

        return ResultData<GameReviewSummaryOutput>.Success(summary);
    }
}
