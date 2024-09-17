using AutoMapper;
using CommonTestUtilities;
using CommonTestUtilities.Mocks.Entities;
using FluentAssertions;
using library.Application.UseCases.Users.GetAll;
using library.Communication.Responses.User;
using library.Domain.Repositories.Users;
using Moq;
using System.Net;

namespace UseCases.Test.Users;

public class GetAllUsersUseCaseTest
{
    private readonly Mock<IUserReadOnlyRepositories> _userRepositoryMock;
    private readonly IMapper _mapper;
    private readonly GetAllUsersUseCase _getAllUsersUseCase;

    public GetAllUsersUseCaseTest()
    {
        _userRepositoryMock = new Mock<IUserReadOnlyRepositories>();
        var mapper = GenerateMapMock.GenerateMock();
        _mapper = mapper;
        _getAllUsersUseCase = new GetAllUsersUseCase(_userRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task ShouldReturnAllUsers()
    {
        // Arrange
        var users = UserBuilder.Builder(10);
        var responseUserJson = _mapper.Map<ICollection<ResponseUserJson>>(users);

        _userRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(users);

        // Act
        var response = await _getAllUsersUseCase.Execute();

        // Assert
        response.Should().NotBeNull();
        response.Data.Count.Should().Be(10);
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenUserRepositoryThrowException()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetAll()).ThrowsAsync(new Exception());

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _getAllUsersUseCase.Execute());

        // Assert
        exception.Should().NotBeNull();
    }
}
