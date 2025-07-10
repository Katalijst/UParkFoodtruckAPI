using Swashbuckle.AspNetCore.Annotations;
using UParkFoodtruckAPI.Models.Enums;

namespace UParkFoodtruckAPI.DTOs;

public class CreateBookingRequest
{
    [SwaggerSchema("Plate of the foodtruck", Nullable = false, Format = "string")]
    public string FoodtruckId { get; set; } = "";
    public ParkingSize Size { get; set; }
    public DateOnly Date { get; set; }
}