using Microsoft.EntityFrameworkCore;
using VehicleInsuranceAPI.Models;

public class VehicleInsuranceDbContext : DbContext
{
    public VehicleInsuranceDbContext(DbContextOptions<VehicleInsuranceDbContext> options) : base(options)
    {
    }

    public DbSet<VehicleInsuranceQuote> VehicleInsuranceQuotes { get; set; }
}
