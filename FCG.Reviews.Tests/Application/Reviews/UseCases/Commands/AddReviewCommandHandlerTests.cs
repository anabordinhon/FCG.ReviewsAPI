using FCG.Reviews.Application.Reviews.Ports;
using FCG.Reviews.Application.Reviews.UseCases.Commands.AddReview;
using FCG.Reviews.Domain.Reviews.Entities;
using FCG.Reviews.Tests.Application.Mocks.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace FCG.Reviews.Tests.Application.Reviews.UseCases.Commands;

public class AddReviewCommandHandlerTests
{
    private readonly Mock<IReviewCommandRepository> _commandRepoMock;
    private readonly Mock<IReviewQueryRepository> _queryRepoMock;
    private readonly AddReviewCommandHandler _handler;

    public AddReviewCommandHandlerTests()
    {
        _commandRepoMock = ReviewCommandRepositoryMock.Create();
        _queryRepoMock = new Mock<IReviewQueryRepository>();
        _handler = new AddReviewCommandHandler(_commandRepoMock.Object, _queryRepoMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarSucesso_QuandoAvaliacaoValida()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var userId = 1;
        var command = new AddReviewCommand(gameId, userId, 5, "Jogo excelente!", DateTime.UtcNow);

        _queryRepoMock
            .Setup(r => r.UserAlreadyReviewedAsync(gameId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Rating.Should().Be(5);
        result.Data.Comment.Should().Be("Jogo excelente!");
    }

    [Fact]
    public async Task Handle_DeveRetornarErro_QuandoUsuarioJaAvaliouJogo()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var userId = 1;
        var command = new AddReviewCommand(gameId, userId, 4, "Segunda avaliação", DateTime.UtcNow);

        _queryRepoMock
            .Setup(r => r.UserAlreadyReviewedAsync(gameId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Você já avaliou este jogo.");
    }

    [Fact]
    public async Task Handle_DeveRetornarErro_QuandoNotaForaDoIntervalo()
    {
        // Arrange
        var command = new AddReviewCommand(Guid.NewGuid(), 1, 6, "Nota inválida", DateTime.UtcNow);

        _queryRepoMock
            .Setup(r => r.UserAlreadyReviewedAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*nota deve ser entre 1 e 5*");
    }
}
