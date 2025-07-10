using UParkFoodtruckAPI.Models.Enums;

namespace UParkFoodtruckAPI.DTOs;

public class MonthlyReportEntry
{
    public string Date { get; set; } = "";
    public int Slot { get; set; }
    public BookingStatus Status { get; set; }
    public decimal Cost { get; set; }
    public bool IsPaid { get; set; }
}