using UParkFoodtruckAPI.Models;

namespace UParkFoodtruckAPI.Repositories;

public interface IBookingRepository
{
    Task AddAsync(Booking reservation);
    Task UpdateAsync(Booking reservation);
    Task<List<Booking>> GetByDateAsync(DateOnly date);
    Task<List<Booking>> GetActiveAsync();
    Task<Booking?> GetByIdAsync(int id);
    Task<List<Booking>> GetByMonthAsync(int year, int month);
}