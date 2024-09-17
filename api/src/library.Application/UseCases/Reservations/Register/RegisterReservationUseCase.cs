using library.Application.Helpers;
using library.Communication.Requests;
using library.Communication.Responses;
using library.Domain.Authentication;
using library.Domain.Entities;
using library.Domain.Enums;
using library.Domain.Repositories;
using library.Domain.Repositories.Books;
using library.Domain.Repositories.Reservations;
using library.Exception.ExceptionBase;
using System.Net;

namespace library.Application.UseCases.Reservations.Register;

public class RegisterReservationUseCase : IRegisterReservationUseCase
{
    private readonly IReservationWriteOnlyRepositories _reservatioRepositories;
    private readonly IBookUpdateOnlyRepositores _bookRepositories;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public RegisterReservationUseCase(
        IReservationWriteOnlyRepositories reservatioRepositories, 
        IUnitOfWork unitOfWork,
        IBookUpdateOnlyRepositores bookRepositories,
        ICurrentUser currentUser)
    {
        _reservatioRepositories = reservatioRepositories;
        _unitOfWork = unitOfWork;
        _bookRepositories = bookRepositories;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse> Execute(Guid bookId)
    {
        var book = await _bookRepositories.GetById(bookId) ?? throw new NotFoundException(Exception.Book.ResourceErrorMessage.BOOK_NOT_FOUND);
        Guid userId = _currentUser.GetId();

        var reservation = new Reservation
        {
            BookId = bookId,
            ReservationCode = GuidHelper.ToShortString(Guid.NewGuid()),
            Status = StatusType.Requested,
            ReservationDate = DateTime.Now,
            Id = Guid.NewGuid(),
            UserId = userId,
            Book = book,
        };

        if (book.Reservations != null && book.Reservations.Count > 0)
        {
            if (book.Reservations.LastOrDefault()?.Status != StatusType.Returned)
            {
                throw new ErrorOnValidationException([Exception.Reservation.ResourceErrorMessage.BOOK_IS_RESERVED]);
            }

            await _reservatioRepositories.Add(reservation);
        }
        else { 
            await _reservatioRepositories.Add(reservation);
        }

        await _unitOfWork.Commit();
        return ApiResponse.CreateSuccesResponse((int)HttpStatusCode.Created);
    }
}
