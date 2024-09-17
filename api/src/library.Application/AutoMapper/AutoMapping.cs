using AutoMapper;
using library.Communication.Requests;
using library.Communication.Responses.Book;
using library.Communication.Responses.BookImage;
using library.Communication.Responses.Reservations;
using library.Communication.Responses.User;
using library.Domain.Entities;

namespace library.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        EntityToResponse();
        RequestToEntity();
    }
    private void RequestToEntity()
    {
        CreateMap<RequestBookJson, Book>();
        CreateMap<RequestUserJson, User>();
    }
    private void EntityToResponse()
    {
        CreateMap<Book, ResponseBookJson>()
            .ForMember(dest => dest.BookImage, opt => opt.MapFrom(src => src.BookImage));
        CreateMap<BookImage, ResponseBookImageJson>();
        CreateMap<Book, ResponseBookGetBySlugJson>();
        CreateMap<Reservation, ResponseReservationJson>()
            .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Book.Title));
        CreateMap<Reservation, ResponseReservationJson>();
        CreateMap<User, ResponseUserJson>();
    }
}
