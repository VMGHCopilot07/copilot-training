using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace VehicleInsuranceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrivingHistoryController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public DrivingHistoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://driving-history-service.com"); // Replace with the actual service URL
        }

        [HttpGet("{driverId}")]
        public async Task<IActionResult> GetDrivingHistory(string driverId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/history/{driverId}");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response (assuming it's in JSON format)
                var drivingHistory = JsonSerializer.Deserialize<DrivingHistory>(responseBody);

                // Extract relevant details
                var driverName = drivingHistory.DriverName;
                var driverLicense = drivingHistory.DriverLicense;
                var birthDate = drivingHistory.BirthDate;
                var emailId = drivingHistory.EmailId;
                var policyId = drivingHistory.PolicyId;
                var policyExpirationDate = drivingHistory.PolicyExpirationDate;

                // Create a DrivingHistoryObject
                var drivingHistoryObject = new DrivingHistoryObject
                {
                    DriverName = driverName,
                    DriverLicense = driverLicense,
                    BirthDate = birthDate,
                    EmailId = emailId,
                    PolicyId = policyId,
                    PolicyExpirationDate = policyExpirationDate
                };

                return Ok(drivingHistoryObject);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error calling external service: {ex.Message}");
            }
        }
    }

    // Define your DrivingHistory and DrivingHistoryObject classes
    public class DrivingHistory
    {
        // Properties representing the response from the service
        public string DriverName { get; set; } = string.Empty;
        public string DriverLicense { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string EmailId { get; set; } = string.Empty;
        public string PolicyId { get; set; } = string.Empty;
        public DateTime PolicyExpirationDate { get; set; }
    }

    public class DrivingHistoryObject
    {
        // Properties for your custom DrivingHistory object
        public string DriverName { get; set; } = string.Empty;
        public string DriverLicense { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string EmailId { get; set; } = string.Empty;
        public string PolicyId { get; set; } = string.Empty;
        public DateTime PolicyExpirationDate { get; set; }
    }
}
