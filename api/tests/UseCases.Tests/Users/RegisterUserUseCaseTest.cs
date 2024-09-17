using AutoMapper;
using CommonTestUtilities;
using CommonTestUtilities.Mocks.Entities;
using CommonTestUtilities.Mocks.Requests;
using FluentAssertions;
using library.Application.Helpers.Cryptography;
using library.Application.UseCases.Users.Register;
using library.Domain.Repositories;
using library.Domain.Repositories.Users;
using library.Exception.ExceptionBase;
using Moq;
using System.Net;

namespace UseCases.Test.Users;

public class RegisterUserUseCaseTest
{
    private readonly Mock<IUserWriteOnlyRepositories> _userWriteRepository;
    private readonly Mock<IUserReadOnlyRepositories> _userReadRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Mock<ICryptography> _cryptography;
    private readonly IRegisterUserUseCase _useCase;

    public RegisterUserUseCaseTest()
    {
        _userWriteRepository = new Mock<IUserWriteOnlyRepositories>();
        _userReadRepository = new Mock<IUserReadOnlyRepositories>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _cryptography = new Mock<ICryptography>();
        var mapper = GenerateMapMock.GenerateMock();
        _mapper = mapper;

        _useCase = new RegisterUserUseCase(
            _userWriteRepository.Object,
            _unitOfWork.Object,
            _userReadRepository.Object,
            _mapper,
            _cryptography.Object);
    }

    [Fact]
    public async Task ShouldRegisterUser()
    {
        // Arrange
        var user = UserBuilder.Builder(1)[0];
        var request = RequestUserJsonBuilder.Builder();
        _userReadRepository.Setup(x => x.GetByEmail(user.Email)).ReturnsAsync(() => null);
        _userWriteRepository.Setup(x => x.AddUser(user));

        // Act
        var response = await _useCase.Execute(request);

        // Assert
        response.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact]
    public async Task ShouldNotRegisterUserWhenUserAlreadyExists()
    {
        // Arrange
        var user = UserBuilder.Builder(1)[0];
        var request = RequestUserJsonBuilder.Builder();
        _userReadRepository.Setup(x => x.GetByEmail(request.Email)).ReturnsAsync(user);

        // Act
        var response = await Assert.ThrowsAsync<ErrorOnValidationException>(() => _useCase.Execute(request));

        // Assert
        response.Should().NotBeNull();
    }
}
