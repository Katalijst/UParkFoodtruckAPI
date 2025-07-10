using UParkFoodtruckAPI.DTOs;
using UParkFoodtruckAPI.Models;
using UParkFoodtruckAPI.Services;

namespace UParkFoodtruckAPI.Endpoints;

public static class BookingsEndpoints
{
    public static void MapReservationEndpoints(this WebApplication app)
    {
        app.MapPost("/booking", async (CreateBookingRequest req, IBookingService service) =>
        {
            var result = await service.CreateBookingAsync(req);
            return result.IsSuccess
                ? Results.Created($"/booking/{result.Value!.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateBooking")
        .WithTags("Bookings")
        .WithSummary("Creates a new booking")
        .WithDescription("Creates a new booking based on the provided reservation details.")
        .Produces<BookingResponse>(StatusCodes.Status201Created)
        .Produces<string>(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        app.MapPost("/booking/cancel/{id:int}", async (int id, IBookingService service) =>
        {
            var result = await service.CancelBookingAsync(id);
            return result.IsSuccess
                ? Results.Ok("Annulation réussie")
                : Results.BadRequest(result.Error);
        })
        .WithName("CancelBooking")
        .WithTags("Bookings")
        .WithSummary("Cancels a booking")
        .WithDescription("Cancels the booking for the given ID.")
        .Produces(StatusCodes.Status200OK)
        .Produces<string>(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        app.MapGet("/booking/active", async (IBookingService service) =>
        {
            var result = await service.GetActiveBookingsAsync();
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("GetActiveBookings")
        .WithTags("Bookings")
        .WithSummary("Retrieves active bookings")
        .WithDescription("Retrieves all active (non-cancelled) bookings.")
        .Produces<List<BookingResponse>>(StatusCodes.Status200OK)
        .Produces<string>(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        app.MapGet("/booking/report/{year:int}/{month:int}", async (int year, int month, IBookingService service) =>
        {
            var result = await service.GenerateMonthlyReportAsync(year, month);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("GetMonthlyReport")
        .WithTags("Bookings")
        .WithSummary("Generates a monthly booking report")
        .WithDescription("Returns a report of all bookings for a given year and month.")
        .Produces<List<MonthlyReportResponse>>(StatusCodes.Status200OK)
        .Produces<string>(StatusCodes.Status400BadRequest)
        .WithOpenApi();
    }
}