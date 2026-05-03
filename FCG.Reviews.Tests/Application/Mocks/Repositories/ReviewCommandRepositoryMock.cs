using FCG.Reviews.Application.Reviews.Ports;
using FCG.Reviews.Domain.Reviews.Entities;
using Moq;

namespace FCG.Reviews.Tests.Application.Mocks.Repositories;

public static class ReviewCommandRepositoryMock
{
    public static Mock<IReviewCommandRepository> Create()
    {
        var mock = new Mock<IReviewCommandRepository>();

        mock.Setup(r => r.AddAsync(It.IsAny<Review>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Review review, CancellationToken _) => review);

        return mock;
    }
}
