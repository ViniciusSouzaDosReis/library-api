using library.Communication.Responses;

namespace library.Application.UseCases.Reservations.Register;
public interface IRegisterReservationUseCase
{
    Task<ApiResponse> Execute(Guid bookId);
}
