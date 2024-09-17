using FluentValidation;
using library.Communication.Requests;
using library.Exception.User;

namespace library.Application.UseCases.Tokens;

public class LoginValidate : AbstractValidator<RequestLoginJson>
{
    public LoginValidate()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.EMAIL_IS_REQUIRED)
            .EmailAddress()
            .WithMessage(ResourceErrorMessage.EMAIL_IS_INVALID);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.PASSWORD_IS_REQUIRED);
    }
}
