using UParkFoodtruckAPI.Models;

namespace UParkFoodtruckAPI.DTOs.Mapping;

public static class BookingResponseMapping
{
    public static BookingResponse ToDto(this Booking booking)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            ParkingSlot = booking.ParkingSlot,
            FoodtruckId = booking.FoodTruckPlate,
            Size = booking.FoodTruckSize,
            Status = booking.Status,
            Date = booking.Date.ToString("yyyy-MM-dd"),
            Cost = booking.Cost,
            IsPaid = booking.IsPaid
        };
    }
}