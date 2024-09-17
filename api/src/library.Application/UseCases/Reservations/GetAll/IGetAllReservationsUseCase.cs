using library.Communication.Responses;
using library.Communication.Responses.Reservations;

namespace library.Application.UseCases.Reservations.GetAll;
public interface IGetAllReservationsUseCase
{
    Task<ApiResponse<ICollection<ResponseReservationJson>>> Execute();
}
