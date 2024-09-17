using Amazon;
using Amazon.S3;
using library.Domain.Authentication;
using library.Domain.Configurations;
using library.Domain.Repositories;
using library.Domain.Repositories.Books;
using library.Domain.Repositories.Images;
using library.Domain.Repositories.Reservations;
using library.Domain.Repositories.Tokens;
using library.Domain.Repositories.Users;
using library.Infrastructure.Authentication;
using library.Infrastructure.Configuration;
using library.Infrastructure.DataAccess;
using library.Infrastructure.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace library.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddContext(services, configuration);
        AddRepositories(services);
        AddConfigureServices(services, configuration);
        AddServices(services, configuration);
    }

    private static void AddContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        var version = new Version(8, 0, 33);
        var serverVersion = new MySqlServerVersion(version);

        services.AddDbContext<LibraryDbContext>(options =>
            options.UseMySql(
                connectionString,
                serverVersion,
                mySqlOptions => mySqlOptions.EnableRetryOnFailure()
        ));
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.AppendTrailingSlash = true;
        });
    }

    private static void AddConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsConfiguration>(configuration.GetSection("AWS"));

        services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));
        services.AddSingleton<IJwtConfiguration>(sp =>
                   sp.GetRequiredService<IOptions<JwtConfiguration>>().Value);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IBookReadOnlyRepositories, BooksRepository>();
        services.AddScoped<IBookWriteOnlyRepositories, BooksRepository>();
        services.AddScoped<IBookUpdateOnlyRepositores, BooksRepository>();
        services.AddScoped<IImageWriteOnlyRepositories, ImagesRepository>();
        services.AddScoped<IReservationWriteOnlyRepositories, ReservationsRepository>();
        services.AddScoped<IReservationReadOnlyRepositories, ReservationsRepository>();
        services.AddScoped<IReservationUpdateOnlyRepositores, ReservationsRepository>();
        services.AddScoped<IUserReadOnlyRepositories, UsersRepository>();
        services.AddScoped<IUserWriteOnlyRepositories, UsersRepository>();
        services.AddScoped<ITokenReadOnlyRepositories, TokenRepository>();
        services.AddScoped<ITokenWriteOnlyRepositories, TokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    }

    private static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        var awsConfiguration = configuration.GetSection("AWS").Get<AwsConfiguration>();
        services.AddSingleton<IAwsConfiguration>(awsConfiguration);

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var awsOptions = sp.GetRequiredService<IAwsConfiguration>();
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(awsOptions.Region)
            };
            return new AmazonS3Client(awsOptions.AccessKey, awsOptions.Secret, config);
        });
    }
}
