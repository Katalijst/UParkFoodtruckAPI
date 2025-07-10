namespace UParkFoodtruckAPI.DTOs;

public class MonthlyReportResponse
{
    public string FoodtruckId { get; set; } = "";
    public List<MonthlyReportEntry> Entries { get; set; } = [];
    public decimal TotalCost { get; set; }
    public decimal TotalPaid { get; set; }
}