using Bogus;
using library.Domain.Entities;

namespace CommonTestUtilities.Mocks.Entities;

public class BookBuilder
{
    public static List<Book> Builder(int quantity)
    {
        var faker = new Faker<Book>();

        return faker.Generate(quantity);
    }
}
