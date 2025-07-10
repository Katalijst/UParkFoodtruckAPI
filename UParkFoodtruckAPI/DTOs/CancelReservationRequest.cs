using Swashbuckle.AspNetCore.Annotations;

namespace UParkFoodtruckAPI.DTOs;

public class CancelReservationRequest
{
    [SwaggerSchema("Id of the booking", Nullable = false, Format = "string")]
    public Guid ReservationId { get; set; }
}