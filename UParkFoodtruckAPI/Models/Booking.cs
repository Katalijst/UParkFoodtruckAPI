using UParkFoodtruckAPI.Models.Enums;

namespace UParkFoodtruckAPI.Models;

public class Booking
{
    public int Id { get; set; }
    public int ParkingSlot { get; set; }
    public string FoodTruckPlate { get; set; } = "";
    public ParkingSize FoodTruckSize { get; set; }
    public BookingStatus Status { get; set; }
    public DateOnly Date { get; set; }
    public decimal Cost { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CancelledAt { get; set; }
    public bool IsPaid { get; set; } = false;
}