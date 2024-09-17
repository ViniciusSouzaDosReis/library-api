using Bogus;
using library.Domain.Entities;

namespace CommonTestUtilities.Mocks.Entities;

public class ReservationBuilder
{
    public static List<Reservation> Builder(int quantity)
    {
        var faker = new Faker<Reservation>();

        return faker.Generate(quantity);
    }
}
