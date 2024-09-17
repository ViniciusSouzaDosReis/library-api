using library.Domain.Entities;
using library.Domain.Repositories.Reservations;
using Microsoft.EntityFrameworkCore;

namespace library.Infrastructure.DataAccess.Repositories;

public class ReservationsRepository : IReservationWriteOnlyRepositories, IReservationReadOnlyRepositories, IReservationUpdateOnlyRepositores
{
    private readonly LibraryDbContext _context;

    public ReservationsRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task Add(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
    }

    public async Task<bool> Delete(Guid id)
    {
       var response = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id);

        if (response is null) return false;

        _context.Reservations.Remove(response);

        return true;
    }

    public async Task<List<Reservation>> GetAll()
    {
        var response = await _context.Reservations.Include(r => r.Book).AsNoTracking().ToListAsync();

        return response;
    }

    public async Task<List<Reservation>> GetAllByUserId(Guid userId)
    {
        var response = await _context.Reservations.Include(r => r.Book).Where(x => x.UserId == userId).AsNoTracking().ToListAsync();

        return response;
    }

    public async Task<Reservation?> GetById(Guid id)
    {
        return await _context.Reservations.Include(r => r.Book).FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
    }
}
