using Bogus;
using library.Domain.Entities;

namespace CommonTestUtilities.Mocks.Entities;

public class UserBuilder
{
    public static List<User> Builder(int quantity)
    {
        var faker = new Faker<User>()
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password());

        return faker.Generate(quantity);
    }
}
