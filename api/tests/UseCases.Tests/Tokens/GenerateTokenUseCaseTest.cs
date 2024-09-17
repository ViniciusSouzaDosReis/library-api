using CommonTestUtilities.Mocks.Entities;
using CommonTestUtilities.Mocks.Requests;
using FluentAssertions;
using library.Application.Helpers.Cryptography;
using library.Application.UseCases.Tokens.Generate;
using library.Communication.Requests;
using library.Domain.Authentication;
using library.Domain.Entities;
using library.Domain.Repositories;
using library.Domain.Repositories.Tokens;
using library.Domain.Repositories.Users;
using library.Exception.ExceptionBase;
using Moq;

namespace UseCases.Test.Tokens;

public class GenerateTokenUseCaseTest
{
    private readonly Mock<IUserReadOnlyRepositories> _userRepository;
    private readonly Mock<ITokenGenerator> _tokenGenerator;
    private readonly Mock<ITokenWriteOnlyRepositories> _tokenWriteRepository;
    private readonly Mock<ITokenReadOnlyRepositories> _tokenReadRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<ICryptography> _cryptography;
    private readonly IGenerateTokenUseCase _useCase;

    public GenerateTokenUseCaseTest()
    {
        _userRepository = new Mock<IUserReadOnlyRepositories>();
        _tokenGenerator = new Mock<ITokenGenerator>();
        _tokenWriteRepository = new Mock<ITokenWriteOnlyRepositories>();
        _tokenReadRepository = new Mock<ITokenReadOnlyRepositories>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _cryptography = new Mock<ICryptography>();

        _useCase = new GenerateTokenUseCase(
            _tokenGenerator.Object,
            _userRepository.Object,
            _tokenWriteRepository.Object,
            _unitOfWork.Object,
            _tokenReadRepository.Object,
            _cryptography.Object);
    }

    [Fact]
    public async Task ShouldReturnTokenWhenCredentialsAreValid()
    {
        // Arrange
        var user = UserBuilder.Builder(1)[0];
        var request = RequestLoginJsonBuilder.Builder();
        _userRepository.Setup(x => x.GetByEmail(request.Email)).ReturnsAsync(user);
        _tokenGenerator.Setup(x => x.Generator(user)).Returns("token_code");
        _tokenReadRepository.Setup(x => x.GetLastByUserId(user.Id)).Returns(() => null);
        _cryptography.Setup(x => x.VerifyHash(request.Password, user.Password)).Returns(true);

        // Act
        var response = await _useCase.Execute(request);

        // Assert
        response.Data.Token.Should().Be("token_code");
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenEmailNotFound()
    {
        // Arrange
        var request = new RequestLoginJson { Email = "nonexistent@email.com", Password = "some_password" };
        _userRepository.Setup(x => x.GetByEmail(request.Email)).ReturnsAsync(() => null);

        // Act & Assert
        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _useCase.Execute(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenPasswordIsInvalid()
    {
        // Arrange
        var user = UserBuilder.Builder(1)[0];
        var request = new RequestLoginJson { Email = user.Email, Password = "invalid_password" };
        _userRepository.Setup(x => x.GetByEmail(user.Email)).ReturnsAsync(user);
        _cryptography.Setup(x => x.VerifyHash(request.Password, user.Password)).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _useCase.Execute(request));
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionWhenRequestIsInvalid()
    {
        // Arrange
        var request = new RequestLoginJson { Email = "", Password = "" }; // Dados inválidos

        // Act & Assert
        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _useCase.Execute(request));
    }

    [Fact]
    public async Task ShouldDeactivateLastActiveToken()
    {
        // Arrange
        var user = UserBuilder.Builder(1)[0];
        var request = RequestLoginJsonBuilder.Builder();
        var lastToken = new Token { IsActived = true, UserId = user.Id };
        _userRepository.Setup(x => x.GetByEmail(request.Email)).ReturnsAsync(user);
        _tokenGenerator.Setup(x => x.Generator(user)).Returns("new_token_code");
        _tokenReadRepository.Setup(x => x.GetLastByUserId(user.Id)).Returns(lastToken);
        _cryptography.Setup(x => x.VerifyHash(request.Password, user.Password)).Returns(true);

        // Act
        await _useCase.Execute(request);

        // Assert
        lastToken.IsActived.Should().BeFalse();
        _tokenWriteRepository.Verify(x => x.Update(lastToken), Times.Once);
    }

    [Fact]
    public async Task ShouldCommitAfterTokenCreation()
    {
        // Arrange
        var user = UserBuilder.Builder(1)[0];
        var request = new RequestLoginJson { Email = user.Email, Password = "valid_password" };
        _userRepository.Setup(x => x.GetByEmail(user.Email)).ReturnsAsync(user);
        _tokenGenerator.Setup(x => x.Generator(user)).Returns("token_code");
        _tokenReadRepository.Setup(x => x.GetLastByUserId(user.Id)).Returns(() => null);
        _cryptography.Setup(x => x.VerifyHash(request.Password, user.Password)).Returns(true);

        // Act
        await _useCase.Execute(request);

        // Assert
        _unitOfWork.Verify(x => x.Commit(), Times.Once);
    }
}
