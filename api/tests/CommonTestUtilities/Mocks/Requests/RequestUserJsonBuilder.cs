using Bogus;
using library.Communication.Requests;

namespace CommonTestUtilities.Mocks.Requests;

public class RequestUserJsonBuilder
{
    public static RequestUserJson Builder()
    {
        return new Faker<RequestUserJson>()
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password());
    }
}
