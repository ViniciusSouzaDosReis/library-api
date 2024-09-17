using CommonTestUtilities.Mocks.Requests;
using FluentAssertions;
using library.Application.UseCases.Users;

namespace Validators.Tests.Users.Register;

public class RegisterUserValidateTests
{
    [Fact]
    public void ShouldReturnTrueWhenRequestIsValid()
    {
        // Arrange
        var validator = new UserValidate();

        var request = RequestUserJsonBuilder.Builder();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldReturnFalseWhenFirstNameIsInvalid(string firstName)
    {
        // Arrange
        var validator = new UserValidate();

        var request = RequestUserJsonBuilder.Builder();
        request.FirstName = firstName;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldReturnFalseWhenLastNameIsInvalid(string lastName)
    {
        // Arrange
        var validator = new UserValidate();

        var request = RequestUserJsonBuilder.Builder();
        request.LastName = lastName;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    public void ShouldReturnFalseWhenEmailIsInvalid(string email)
    {
        // Arrange
        var validator = new UserValidate();

        var request = RequestUserJsonBuilder.Builder();
        request.Email = email;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("12345")]
    public void ShouldReturnFalseWhenPasswordIsInvalid(string password)
    {
        // Arrange
        var validator = new UserValidate();

        var request = RequestUserJsonBuilder.Builder();
        request.Password = password;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}

