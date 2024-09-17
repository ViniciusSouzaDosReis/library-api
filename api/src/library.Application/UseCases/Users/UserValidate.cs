using FluentValidation;
using library.Communication.Requests;
using library.Exception.User;

namespace library.Application.UseCases.Users;

public class UserValidate : AbstractValidator<RequestUserJson>
{
    public UserValidate()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.NAME_IS_REQUIRED);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.LAST_NAME_IS_REQUIRED);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.EMAIL_IS_REQUIRED)
            .EmailAddress()
            .WithMessage(ResourceErrorMessage.EMAIL_IS_INVALID);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.PASSWORD_IS_REQUIRED)
            .MinimumLength(6)
            .WithMessage(ResourceErrorMessage.PASSWORD_MUST_BE_AT_LEAST_6_CHARACTERS);
    }
}
