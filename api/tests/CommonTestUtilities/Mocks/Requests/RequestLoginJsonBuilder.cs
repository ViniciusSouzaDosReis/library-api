using Bogus;
using library.Communication.Requests;

namespace CommonTestUtilities.Mocks.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Builder()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password());
    }
}
