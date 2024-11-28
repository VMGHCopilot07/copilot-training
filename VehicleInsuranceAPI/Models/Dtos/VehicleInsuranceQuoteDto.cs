public class VehicleInsuranceQuoteDto
{
    public int QuoteId { get; set; }
    public string VehicleMake { get; set; }
    public string VehicleModel { get; set; }
    public int VehicleYear { get; set; }
    public string VehicleVIN { get; set; }
    public string VehicleType { get; set; }
    public int VehicleMileage { get; set; }
    public string OwnerName { get; set; }
    public string OwnerAddress { get; set; }
    public decimal PremiumAmount { get; set; }
    public string Rating { get; set; }
}
