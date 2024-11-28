using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ClaimsHistoryService
{
    private readonly HttpClient _httpClient;

    public ClaimsHistoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ClaimsHistoryDto?> GetClaimsHistoryAsync(string driverName)
    {
        // Replace with the actual URL of the third-party dummy service
        var url = $"https://dummyapi.com/claims-history?driverName={driverName}";
        return await _httpClient.GetFromJsonAsync<ClaimsHistoryDto>(url);
    }
}

public class ClaimsHistoryDto
{
    public string DriverName { get; set; } = string.Empty;
    public int NumberOfClaims { get; set; }
    public decimal TotalClaimAmount { get; set; }
}

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddDbContext<VehicleInsuranceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddHttpClient<ClaimsHistoryService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
