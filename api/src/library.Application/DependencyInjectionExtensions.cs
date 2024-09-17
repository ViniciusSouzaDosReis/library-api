using library.Application.AutoMapper;
using library.Application.Helpers.Cryptography;
using library.Application.UseCases.BookImages.UploadImage;
using library.Application.UseCases.Books.Delete;
using library.Application.UseCases.Books.GetAll;
using library.Application.UseCases.Books.GetById;
using library.Application.UseCases.Books.GetBySlug;
using library.Application.UseCases.Books.Register;
using library.Application.UseCases.Books.Update;
using library.Application.UseCases.Reservations.Delete;
using library.Application.UseCases.Reservations.GetAll;
using library.Application.UseCases.Reservations.Pick;
using library.Application.UseCases.Reservations.Register;
using library.Application.UseCases.Reservations.Request;
using library.Application.UseCases.Reservations.Return;
using library.Application.UseCases.Tokens.Generate;
using library.Application.UseCases.Tokens.Logout;
using library.Application.UseCases.Users.Delete;
using library.Application.UseCases.Users.GetAll;
using library.Application.UseCases.Users.Register;
using library.Application.UseCases.Utils.Image;
using library.Application.UseCases.Utils.Reservations;
using Microsoft.Extensions.DependencyInjection;

namespace library.Application;

public static class DependencyInjectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMaping(services);
        AddUseCases(services);
        AddUtils(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IGetAllBooksUseCase, GetAllBooksUseCase>();
        services.AddScoped<IRegisterBookUseCase, RegisterBookUseCase>();
        services.AddScoped<IUpdateBookUseCase, UpdateBookUseCase>();
        services.AddScoped<IGetByIdBookUseCase, GetByIdBookUseCase>();
        services.AddScoped<IGetBySlugBookUseCase, GetBySlugBookUseCase>();
        services.AddScoped<IUploadBookImageUseCase, UploadBookImageUseCase>();
        services.AddScoped<IDeleteBookUseCase, DeleteBookUseCase>();
        services.AddScoped<IRegisterReservationUseCase, RegisterReservationUseCase>();
        services.AddScoped<IGetAllReservationsUseCase, GetAllReservationsUseCase>();
        services.AddScoped<IDeleteReservationUseCase, DeleteReservationUseCase>();
        services.AddScoped<IReturnReservationUseCase, ReturnReservationUseCase>();
        services.AddScoped<IPickReservationUseCase, PickReservationUseCase>();
        services.AddScoped<IRequestBookUseCase, RequestBookUseCase>();
        services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddScoped<IGenerateTokenUseCase, GenerateTokenUseCase>();
        services.AddScoped<ILogoutTokenUseCase, LogoutTokenUseCase>();
    }

    private static void AddUtils(IServiceCollection services)
    {
        services.AddScoped<IImageUtils, ImageUtils>();
        services.AddScoped<IReservationsUtils, ReservationsUtils>();
        services.AddScoped<ICryptography, Cryptography>();
    }

    private static void AddAutoMaping(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }
}
