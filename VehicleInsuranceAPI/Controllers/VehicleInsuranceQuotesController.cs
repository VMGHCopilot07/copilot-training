using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleInsuranceAPI.Models;
using VehicleInsuranceAPI.Models.Dtos;

[Route("api/[controller]")]
[ApiController]
public class VehicleInsuranceQuotesController : ControllerBase
{
    private readonly VehicleInsuranceDbContext _context;

    public VehicleInsuranceQuotesController(VehicleInsuranceDbContext context)
    {
        _context = context;
    }

    // GET: api/VehicleInsuranceQuotes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleInsuranceQuoteDto>>> GetVehicleInsuranceQuotes()
    {
        return await _context.VehicleInsuranceQuotes
            .Select(q => new VehicleInsuranceQuoteDto
            {
                QuoteId = q.QuoteId,
                VehicleMake = q.VehicleMake,
                VehicleModel = q.VehicleModel,
                VehicleYear = q.VehicleYear,
                VehicleVIN = q.VehicleVIN,
                VehicleType = q.VehicleType,
                VehicleMileage = q.VehicleMileage,
                OwnerName = q.OwnerName,
                OwnerAddress = q.OwnerAddress,
                PremiumAmount = q.PremiumAmount,
                Rating = q.Rating
            })
            .ToListAsync();
    }

    // GET: api/VehicleInsuranceQuotes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleInsuranceQuoteDto>> GetVehicleInsuranceQuote(int id)
    {
        var quote = await _context.VehicleInsuranceQuotes.FindAsync(id);

        if (quote == null)
        {
            return NotFound();
        }

        var quoteDto = new VehicleInsuranceQuoteDto
        {
            QuoteId = quote.QuoteId,
            VehicleMake = quote.VehicleMake,
            VehicleModel = quote.VehicleModel,
            VehicleYear = quote.VehicleYear,
            VehicleVIN = quote.VehicleVIN,
            VehicleType = quote.VehicleType,
            VehicleMileage = quote.VehicleMileage,
            OwnerName = quote.OwnerName,
            OwnerAddress = quote.OwnerAddress,
            PremiumAmount = quote.PremiumAmount,
            Rating = quote.Rating
        };

        return quoteDto;
    }

    // POST: api/VehicleInsuranceQuotes
    [HttpPost]
    public async Task<ActionResult<VehicleInsuranceQuote>> PostVehicleInsuranceQuote(VehicleInsuranceQuoteDto quoteDto)
    {
        var quote = new VehicleInsuranceQuote
        {
            QuoteId = quoteDto.QuoteId,
            VehicleMake = quoteDto.VehicleMake,
            VehicleModel = quoteDto.VehicleModel,
            VehicleYear = quoteDto.VehicleYear,
            VehicleVIN = quoteDto.VehicleVIN,
            VehicleType = quoteDto.VehicleType,
            VehicleMileage = quoteDto.VehicleMileage,
            OwnerName = quoteDto.OwnerName,
            OwnerAddress = quoteDto.OwnerAddress,
            PremiumAmount = quoteDto.PremiumAmount,
            Rating = quoteDto.Rating
        };

        _context.VehicleInsuranceQuotes.Add(quote);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVehicleInsuranceQuote), new { id = quote.QuoteId }, quote);
    }

    // PUT: api/VehicleInsuranceQuotes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehicleInsuranceQuote(int id, VehicleInsuranceQuoteDto quoteDto)
    {
        if (id != quoteDto.QuoteId)
        {
            return BadRequest();
        }

        var quote = await _context.VehicleInsuranceQuotes.FindAsync(id);
        if (quote == null)
        {
            return NotFound();
        }

        quote.VehicleMake = quoteDto.VehicleMake;
        quote.VehicleModel = quoteDto.VehicleModel;
        quote.VehicleYear = quoteDto.VehicleYear;
        quote.VehicleVIN = quoteDto.VehicleVIN;
        quote.VehicleType = quoteDto.VehicleType;
        quote.VehicleMileage = quoteDto.VehicleMileage;
        quote.OwnerName = quoteDto.OwnerName;
        quote.OwnerAddress = quoteDto.OwnerAddress;
        quote.PremiumAmount = quoteDto.PremiumAmount;
        quote.Rating = quoteDto.Rating;

        _context.Entry(quote).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/VehicleInsuranceQuotes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicleInsuranceQuote(int id)
    {
        var quote = await _context.VehicleInsuranceQuotes.FindAsync(id);
        if (quote == null)
        {
            return NotFound();
        }

        _context.VehicleInsuranceQuotes.Remove(quote);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
