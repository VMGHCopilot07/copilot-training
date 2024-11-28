using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using VehicleInsuranceAPI.Models;

namespace VehicleInsuranceAPI.Controllers
{
    /// <summary>
    /// BillController handles all the operations related to customer bills.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly VipDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="BillController"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        public BillController(VipDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets the list of all customer bills.
        /// </summary>
        /// <returns>A list of customer bills.</returns>
        /// <response code="200">Returns the list of customer bills</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerBill>>> GetCustomerBills()
        {
            return await _db.CustomerBills.ToListAsync();
        }

        /// <summary>
        /// Gets the bill number for a specific policy number if the bill status is "Completed".
        /// </summary>
        /// <param name="policyNo">The policy number.</param>
        /// <returns>The bill number or -1 if not found.</returns>
        /// <response code="200">Returns the bill number</response>
        /// <response code="404">If the bill is not found</response>
        [HttpGet("{policyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Bill(int policyNo)
        {
            var customerBill = _db.CustomerBills.Where(b => b.PolicyNo == policyNo && b.Status.Equals("Completed")).FirstOrDefault();

            if (customerBill == null)
            {
                return Ok(-1);
            }

            return Ok(customerBill.BillNo);
        }

        /// <summary>
        /// Updates a specific customer bill.
        /// </summary>
        /// <param name="id">The ID of the customer bill to update.</param>
        /// <param name="customerBill">The updated customer bill.</param>
        /// <returns>No content if successful, otherwise a relevant error response.</returns>
        /// <response code="204">If the update is successful</response>
        /// <response code="400">If the ID does not match the customer bill</response>
        /// <response code="404">If the customer bill is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCustomerBill(int id, CustomerBill customerBill)
        {
            if (id != customerBill.Id)
            {
                return BadRequest();
            }

            _db.Entry(customerBill).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerBillExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new customer bill.
        /// </summary>
        /// <param name="model">The customer bill model.</param>
        /// <returns>The bill number if successful, otherwise -1 or a relevant error response.</returns>
        /// <response code="200">Returns the bill number</response>
        /// <response code="204">If the certificate is not found</response>
        /// <response code="400">If there is an error during creation</response>
        [HttpPost]
        [Route("PostCustomerBill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostCustomerBill(CustomerBillModel model)
        {
            try
            {
                Certificate certificate = _db.Certificates.Where(c => c.PolicyNo == model.PolicyNo).FirstOrDefault();
                if (certificate == null)
                {
                    return NoContent();
                }

                certificate.VehicleWarranty = "Pending";
                CustomerBill bill = new CustomerBill()
                {
                    BillNo = model.BillNo,
                    PolicyNo = model.PolicyNo,
                    Status = model.Status,
                    Date = model.Date,
                    Amount = model.Amount,
                    PolicyNoNavigation = certificate
                };
                _db.CustomerBills.Add(bill);
                _db.Entry(bill.PolicyNoNavigation).State = EntityState.Modified;
                if (_db.SaveChanges() > 0)
                {
                    _db.Dispose();
                    return Ok(model.BillNo);
                }
                return Ok(-1);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            finally
            {
                _db.Dispose();
            }
        }

        /// <summary>
        /// Deletes a specific customer bill.
        /// </summary>
        /// <param name="id">The ID of the customer bill to delete.</param>
        /// <returns>No content if successful, otherwise a relevant error response.</returns>
        /// <response code="204">If the deletion is successful</response>
        /// <response code="404">If the customer bill is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomerBill(int id)
        {
            var customerBill = await _db.CustomerBills.FindAsync(id);
            if (customerBill == null)
            {
                return NotFound();
            }

            _db.CustomerBills.Remove(customerBill);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a customer bill exists.
        /// </summary>
        /// <param name="id">The ID of the customer bill.</param>
        /// <returns>True if the customer bill exists, otherwise false.</returns>
        private bool CustomerBillExists(int id)
        {
            return _db.CustomerBills.Any(e => e.Id == id);
        }
    }
}
