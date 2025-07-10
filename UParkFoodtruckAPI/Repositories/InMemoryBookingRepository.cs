using UParkFoodtruckAPI.Models;
using UParkFoodtruckAPI.Models.Enums;

namespace UParkFoodtruckAPI.Repositories;

public class InMemoryBookingRepository : IBookingRepository
{
    private static int _idSequence = 1;
    private static readonly object _lock = new();
    private readonly Dictionary<int, Booking> _bookings = new();

    public Task AddAsync(Booking reservation)
    {
        lock (_lock)
        {
            reservation.Id = _idSequence++;
            _bookings.Add(reservation.Id, reservation);
        }
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Booking reservation)
    {
        if (_bookings.ContainsKey(reservation.Id))
        {
            _bookings[reservation.Id] = reservation;
        }
        else
        {
            throw new KeyNotFoundException($"Booking with ID {reservation.Id} not found.");
        }
        return Task.CompletedTask;
    }

    public Task<List<Booking>> GetByDateAsync(DateOnly date)
    {
        var result = _bookings.Values
            .Where(b => b.Date == date && b.Status != BookingStatus.Cancelled)
            .ToList();
        return Task.FromResult(result);
    }

    public Task<List<Booking>> GetActiveAsync()
    {
        var result = _bookings.Values
            .Where(b => b.Status != BookingStatus.Cancelled)
            .ToList();
        return Task.FromResult(result);
    }

    public Task<Booking?> GetByIdAsync(int id)
    {
        _bookings.TryGetValue(id, out var booking);
        return Task.FromResult(booking);
    }

    public Task<List<Booking>> GetByMonthAsync(int year, int month)
    {
        var result = _bookings.Values
            .Where(b => b.Date.Year == year && b.Date.Month == month)
            .ToList();
        return Task.FromResult(result);
    }
}
