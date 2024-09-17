using Bogus;
using library.Communication.Requests;

namespace CommonTestUtilities.Mocks.Requests;

public class RequestBookJsonBuilder
{
    public static RequestBookJson Builder()
    {
        return new Faker<RequestBookJson>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.Author, f => f.Name.FullName())
            .RuleFor(x => x.Synopsis, f => f.Lorem.Paragraph())
            .RuleFor(x => x.Genres, f => f.Lorem.Words(3))
            .RuleFor(x => x.Language, f => f.Lorem.Word())
            .RuleFor(x => x.Published, f => f.Date.Past(5));
    }
}
