namespace FCG.Reviews.Application.Reviews.Outputs;

public record ReviewOutput(
    string Id,
    Guid GameId,
    int UserId,
    int Rating,
    string Comment,
    DateTime CreatedAt
);
