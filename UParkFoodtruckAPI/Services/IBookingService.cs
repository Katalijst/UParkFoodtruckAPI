using UParkFoodtruckAPI.Common;
using UParkFoodtruckAPI.DTOs;
using UParkFoodtruckAPI.Models;

namespace UParkFoodtruckAPI.Services;

public interface IBookingService
{
    Task<Result<BookingResponse>> CreateBookingAsync(CreateBookingRequest req);
    Task<Result<BookingResponse>> CancelBookingAsync(int id);
    Task<Result<List<BookingResponse>>> GetActiveBookingsAsync();
    Task<Result<List<MonthlyReportResponse>>> GenerateMonthlyReportAsync(int year, int month);
}