using FluentValidation;
using library.Communication.Requests;
using library.Exception.Book;

namespace library.Application.UseCases.Books;

public class BookValidate : AbstractValidator<RequestBookJson>
{
    public BookValidate()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.TIITLE_IS_REQUIRED);

        RuleFor(x => x.Author)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.AUTHOR_IS_REQUIRED);

        RuleFor(x => x.Synopsis)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.SYNOPSIS_IS_REQUIRED);

        RuleFor(x => x.Published)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.PUBLISHED_IS_REQUIRED)
            .LessThan(DateTime.Now)
            .WithMessage(ResourceErrorMessage.PUBLISHED_DATE_MUST_BE_LESS_THAN_TODAY);

        RuleFor(x => x.Genres)
            .NotNull().WithMessage(ResourceErrorMessage.GENRE_IS_REQUIRED)
            .ForEach(x => x.NotEmpty().WithMessage(ResourceErrorMessage.GENRE_CANT_EMPTY));

        RuleFor(x => x.Language)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.LANGUAGE_IS_REQUIRED);
    }
}
