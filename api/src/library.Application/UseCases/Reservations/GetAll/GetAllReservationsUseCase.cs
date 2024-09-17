using AutoMapper;
using library.Communication.Responses;
using library.Communication.Responses.Reservations;
using library.Domain.Authentication;
using library.Domain.Entities;
using library.Domain.Enums;
using library.Domain.Repositories.Reservations;
using System.Net;

namespace library.Application.UseCases.Reservations.GetAll;

public class GetAllReservationsUseCase : IGetAllReservationsUseCase
{
    private readonly IReservationReadOnlyRepositories _reservationRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllReservationsUseCase(IReservationReadOnlyRepositories reservationRepository, IMapper mapper, ICurrentUser currentUser)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<ICollection<ResponseReservationJson>>> Execute()
    {
        List<Reservation> reservations;
        Guid userId = _currentUser.GetId();
        string role = _currentUser.GetRole();

        if (role == RoleType.Admin.ToString())
        {
            reservations = await _reservationRepository.GetAll();
        }
        else
        {
            reservations = await _reservationRepository.GetAllByUserId(userId);
        }

        var response = ApiResponse.CreateSuccesResponseWithData(
            _mapper.Map<ICollection<ResponseReservationJson>>(reservations),
            (int)HttpStatusCode.OK);

        return response;
    }
}
