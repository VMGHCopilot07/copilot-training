using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VehicleInsuranceAPI.Models;

namespace VehicleInsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerBillController : ControllerBase
    {
        private readonly VipDbContext _db;
        public CustomerBillController(VipDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get all Customer Bill
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCustomerBill")]
        public IActionResult GetAllCustomerBill()
        {
            List<CustomerBillModel> model = new List<CustomerBillModel>();
            model = (from cusbill in _db.CustomerBills
                     join cer in _db.Certificates on cusbill.PolicyNo equals cer.PolicyNo
                     select new CustomerBillModel
                     {
                         Id = cusbill.Id,
                         BillNo = cusbill.BillNo,
                         Status = cusbill.Status,
                         Date = cusbill.Date,
                         PolicyNo = cusbill.PolicyNo,
                         Amount = cusbill.Amount
                     }).ToList();

            //from cusbill in _db.CustomerBills 
            //     join est in _db.Estimates on cusbill.Id equals est.Id
            //     select new CustomerBillModel
            //     {
            //         Id = cusbill.Id,
            //         BillNo = cusbill.BillNo,
            //         Status = cusbill.Status,
            //         Date = cusbill.Date,
            //         PolicyNo = cusbill.PolicyNo,
            //         Amount = est.Premium,
            //         //Premium = est.Premium,
            //     }).ToList();
            return Ok(model);
        }

        [HttpGet]
        [Route("GetDetail/{BillId}")]
        public async Task<CustomerBillViewModel> GetDetail(int BillId)
        {
            var records = await _db.CustomerBills.Where(c => c.Id == BillId).Select(c => new CustomerBillViewModel()
            {
                Id = c.Id,
                BillNo = c.BillNo,
                Status = c.Status,
                Date = c.Date,
                PolicyNo = c.PolicyNo,
                Amount = c.Amount,
            }).FirstOrDefaultAsync();

            return records;

        }

        [HttpPost]
        [Route("UpdateBill")]
        public async Task UpdateBill(CustomerBillViewModel model)
        {
            var bill = new CustomerBill()
            {
                Id = model.Id,
                BillNo= model.BillNo,
                Status = model.SelectedStatus,
                Date = model.Date,
                PolicyNo = model.PolicyNo,
                Amount = model.Amount,
            };

            _db.CustomerBills.Update(bill);
            await _db.SaveChangesAsync();
        }

        public double CalculateTotalPremium(double basePremium, double riskFactor, int insurerAge, string policyType, string vehicleType)
        {
            // Poor coding standards: No validation checks, no comments, and poor naming conventions
            double totalPremium = basePremium;

            // Separate if-else statements for each condition (anti-pattern)
            if (policyType == "Comprehensive")
            {
                if (vehicleType == "SUV")
                    totalPremium *= 1.2;
            }
            else if (policyType == "ThirdParty")
            {
                if (vehicleType == "Sedan")
                    totalPremium *= 1.1;
            }

            // Adjust premium based on insurer's age (magic numbers)
            if (insurerAge < 25)
                totalPremium *= 1.3;
            else if (insurerAge >= 60)
                totalPremium *= 0.9;

            // Apply risk factor (no comments or explanation)
            totalPremium *= riskFactor;

            // Poor naming conventions: No meaningful variable names
            return totalPremium;
        }

        public void CalculatePremium(string[] args)
        {
            // Sample data
            string[] regions = { "North", "South", "East", "West" };
            string[] agents = { "Agent1", "Agent2", "Agent3" };
            string[] customers = { "Customer1", "Customer2", "Customer3" };
            string[] policies = { "Health", "Life", "Auto", "Home" };

            // Nested loops to calculate total premium
            double totalPremium = 0.0;

            foreach (var region in regions)
            {
                foreach (var agent in agents)
                {
                    foreach (var customer in customers)
                    {
                        foreach (var policy in policies)
                        {
                            // Sample premium calculation
                            double premium = CalculatePremium(region, agent, customer, policy);
                            totalPremium += premium;

                            Console.WriteLine($"Region: {region}, Agent: {agent}, Customer: {customer}, Policy: {policy}, Premium: {premium:C}");
                        }
                    }
                }
            }

            Console.WriteLine($"\nTotal Premium: {totalPremium:C}");
        }

        private double CalculatePremium(string region, string agent, string customer, string policy)
        {
            // Sample premium calculation logic
            return 100.0; // Placeholder value
        }
    }
}
