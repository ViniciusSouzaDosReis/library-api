using library.Application.UseCases.Books;
using FluentAssertions;
using CommonTestUtilities.Mocks.Requests;

namespace Validators.Tests.Books.Register;

public class RegisterBookValidateTests
{
    [Fact]
    public void ShouldReturnTrueWhenRequestIsValid()
    {
        // Arrange
        var validator = new BookValidate();

        var request = RequestBookJsonBuilder.Builder();
        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldReturnFalseWhenTitleIsEmpty(string title)
    {
        // Arrange
        var validator = new BookValidate();

        var request = RequestBookJsonBuilder.Builder();
        request.Title = title;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldReturnFalseWhenAuthorIsEmpty(string author)
    {
        // Arrange
        var validator = new BookValidate();

        var request = RequestBookJsonBuilder.Builder();
        request.Author = author;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData(new object[] { new string[] { "" }})]
    [InlineData(new object[] { new string[] { " " } })]
    [InlineData(new object[] { new string[] { "teste", "" } })]
    public void ShouldReturnFalseWhenGenreIsEmpty(string[] genres)
    {
        // Arrange
        var validator = new BookValidate();

        var request = RequestBookJsonBuilder.Builder();
        request.Genres = genres;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalseWhenPublishedDateIsGreaterThanToday()
    {
        // Arrange
        var validator = new BookValidate();

        var request = RequestBookJsonBuilder.Builder();
        request.Published = DateTime.Now.AddDays(1);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalseWhenPublishedDateIsNull()
    {
        // Arrange
        var validator = new BookValidate();

        var request = RequestBookJsonBuilder.Builder();
        request.Published = null;   

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldReturnFalseWhenLanguageIsEmpty(string language)
    {
        // Arrange
        var validator = new BookValidate();

        var request = RequestBookJsonBuilder.Builder();
        request.Language = language;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldReturnFalseWhenSynopsisIsEmpty(string synopsis)
    {
        // Arrange
        var validator = new BookValidate();

        var request = RequestBookJsonBuilder.Builder();
        request.Synopsis = synopsis;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}