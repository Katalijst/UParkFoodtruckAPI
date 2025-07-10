using UParkFoodtruckAPI.Common;
using UParkFoodtruckAPI.DTOs;
using UParkFoodtruckAPI.DTOs.Mapping;
using UParkFoodtruckAPI.Models;
using UParkFoodtruckAPI.Models.Enums;
using UParkFoodtruckAPI.Repositories;

namespace UParkFoodtruckAPI.Services;

public class BookingService : IBookingService
{
    private const int MaxSlots = 7;
    private const int BasePrice = 20;
    private readonly ILogger<BookingService> _logger;
    private readonly IBookingRepository _repo;

    public BookingService(IBookingRepository repo, ILogger<BookingService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Result<BookingResponse>> CreateBookingAsync(CreateBookingRequest req)
    {
        try
        {
            var now = DateOnly.FromDateTime(DateTime.UtcNow);
            var earliestBookingDate = now.AddDays(-7);

            if (req.Date < now) return Result<BookingResponse>.Fail("Vous ne pouvez pas réserver une date dans le passé.");
            if (req.Date < earliestBookingDate)
                return Result<BookingResponse>.Fail("Vous ne pouvez pas réserver plus de 7 jours à l'avance.");

            var existing = await _repo.GetByDateAsync(req.Date);
            var availableSlot = FindAvailableSlot(existing, req);
            if (availableSlot is null)
                return Result<BookingResponse>.Fail("Aucune place disponible pour cette date.");

            var cost = CalculateCost(req.Date);

            var booking = new Booking
            {
                ParkingSlot = availableSlot.Value,
                FoodTruckPlate = req.FoodtruckId,
                FoodTruckSize = req.Size,
                Status = BookingStatus.Valid,
                Date = req.Date,
                Cost = cost,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(booking);
            return Result<BookingResponse>.Success(booking.ToDto());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erreur lors de la création de la réservation");
            return Result<BookingResponse>.Fail("Une erreur est survenue lors de la création de la réservation.");
        }
    }

    public async Task<Result<BookingResponse>> CancelBookingAsync(int id)
    {
        try
        {
            var reservation = await _repo.GetByIdAsync(id);
            if (reservation is not { Status: BookingStatus.Valid })
                return Result<BookingResponse>.Fail("Réservation introuvable ou déjà annulée");

            var daysBefore = (DateTime.UtcNow - reservation.Date.ToDateTime(TimeOnly.MinValue)).TotalDays;

            reservation.Status = BookingStatus.Cancelled;
            reservation.CancelledAt = DateTime.UtcNow;
            if (daysBefore >= 2) reservation.Cost = 0; // Si plus de 2 jours avant, remboursement complet
            await _repo.UpdateAsync(reservation);
            return Result<BookingResponse>.Success(reservation.ToDto());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erreur lors de l'annulation de la réservation");
            return Result<BookingResponse>.Fail("Une erreur est survenue lors de l'annulation de la réservation.");
        }
    }

    public async Task<Result<List<BookingResponse>>> GetActiveBookingsAsync()
    {
        try
        {
            var bookings = await _repo.GetActiveAsync();
            var dtoList = bookings.Select(b => b.ToDto()).ToList();
            return Result<List<BookingResponse>>.Success(dtoList);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erreur lors de la récupération des réservations actives");
            return Result<List<BookingResponse>>.Fail(
                "Une erreur est survenue lors de la récupération des réservations actives.");
        }
    }

    public async Task<Result<List<MonthlyReportResponse>>> GenerateMonthlyReportAsync(int year, int month)
    {
        try
        {
            var bookings = (await _repo.GetByMonthAsync(year, month)).OrderBy(b => b.FoodTruckPlate);
            var report = new List<MonthlyReportResponse>();
            var lastTruckReport = new MonthlyReportResponse();

            if (!bookings.Any()) return Result<List<MonthlyReportResponse>>.Success(report);

            foreach (var booking in bookings)
            {
                if (lastTruckReport.FoodtruckId != booking.FoodTruckPlate)
                {
                    if (!string.IsNullOrWhiteSpace(lastTruckReport.FoodtruckId)) report.Add(lastTruckReport);
                    lastTruckReport = new MonthlyReportResponse
                    {
                        FoodtruckId = booking.FoodTruckPlate,
                        Entries = [],
                        TotalCost = 0
                    };
                }

                lastTruckReport.Entries.Add(
                    new MonthlyReportEntry
                    {
                        Date = booking.Date.ToString("yyyy-MM-dd"),
                        Slot = booking.ParkingSlot,
                        Status = booking.Status,
                        Cost = booking.Cost,
                        IsPaid = booking.IsPaid
                    });
                lastTruckReport.TotalCost += booking.Cost;
                if (booking.IsPaid)
                    lastTruckReport.TotalPaid += booking.Cost;
            }

            if (!string.IsNullOrWhiteSpace(lastTruckReport.FoodtruckId)) report.Add(lastTruckReport);
            return Result<List<MonthlyReportResponse>>.Success(report);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erreur lors de la génération du rapport mensuel");
            return Result<List<MonthlyReportResponse>>.Fail(
                "Une erreur est survenue lors de la génération du rapport mensuel.");
        }
    }

    private static int? FindAvailableSlot(List<Booking> reservations, CreateBookingRequest req)
    {
        var fridayBlocked = req.Date.DayOfWeek == DayOfWeek.Friday;
        if (fridayBlocked)
            return null; // No reservations allowed on Friday

        for (var slot = 1; slot <= MaxSlots; slot++)
        {
            var sameSlot = reservations.Where(r => r.ParkingSlot == slot).ToList();
            var totalSize = sameSlot.Sum(r => (int)r.FoodTruckSize);

            if (totalSize + (int)req.Size <= (int)ParkingSize.Full)
                return slot;
        }

        return null;
    }

    private static decimal CalculateCost(DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return date > today ? BasePrice : BasePrice * 2;
    }
}