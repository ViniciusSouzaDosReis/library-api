using AutoMapper;
using library.Application.AutoMapper;

namespace CommonTestUtilities;

public class GenerateMapMock
{
    public static Mapper GenerateMock()
    {
        var myProfile = new AutoMapping();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        return mapper;
    }
}
