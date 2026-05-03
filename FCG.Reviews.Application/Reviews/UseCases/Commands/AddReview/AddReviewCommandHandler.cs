using FCG.Reviews.Application.Common;
using FCG.Reviews.Application.Reviews.Mappers;
using FCG.Reviews.Application.Reviews.Outputs;
using FCG.Reviews.Application.Reviews.Ports;
using FCG.Reviews.Domain.Reviews.Entities;

namespace FCG.Reviews.Application.Reviews.UseCases.Commands.AddReview;

public class AddReviewCommandHandler : IAddReviewCommandHandler
{
    private readonly IReviewCommandRepository _commandRepository;
    private readonly IReviewQueryRepository _queryRepository;

    public AddReviewCommandHandler(IReviewCommandRepository commandRepository, IReviewQueryRepository queryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    public async Task<ResultData<ReviewOutput>> Handle(AddReviewCommand command, CancellationToken cancellationToken)
    {
        var alreadyReviewed = await _queryRepository.UserAlreadyReviewedAsync(
            command.GameId, command.UserId, cancellationToken);

        if (alreadyReviewed)
            return ResultData<ReviewOutput>.Error("Você já avaliou este jogo.");

        var review = Review.Create(command.GameId, command.UserId, command.Rating, command.Comment, command.CreatedAt);

        await _commandRepository.AddAsync(review, cancellationToken);

        return ResultData<ReviewOutput>.Success(review.ToOutput());
    }
}
