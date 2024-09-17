using CommonTestUtilities.Mocks.Requests;
using FluentAssertions;
using library.Application.UseCases.Tokens;

namespace Validators.Tests.Tokens.Generate;

public class LoginValidateTests
{
    [Fact]
    public void ShouldReturnTrueWhenRequestIsValid()
    {
        // Arrange
        var validator = new LoginValidate();

        var request = RequestLoginJsonBuilder.Builder();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    public void ShouldReturnFalseWhenEmailIsInvalid(string email)
    {
        // Arrange
        var validator = new LoginValidate();

        var request = RequestLoginJsonBuilder.Builder();
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
    public void ShouldReturnFalseWhenPasswordIsEmpty(string password)
    {
        // Arrange
        var validator = new LoginValidate();

        var request = RequestLoginJsonBuilder.Builder();
        request.Password = password;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}