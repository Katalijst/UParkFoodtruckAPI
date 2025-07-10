using Swashbuckle.AspNetCore.Annotations;
using UParkFoodtruckAPI.Models.Enums;

namespace UParkFoodtruckAPI.DTOs;

public class BookingResponse
{
    public int Id { get; set; }
    [SwaggerSchema("Parking slot from 1 to 7", Nullable = false, Format = "string")]
    public int ParkingSlot { get; set; }
    public string FoodtruckId { get; set; } = string.Empty;
    public ParkingSize Size { get; set; } = ParkingSize.Full;
    public BookingStatus Status { get; set; } = BookingStatus.Valid;
    public string Date { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public bool IsPaid { get; set; }
}