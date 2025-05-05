using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VillaRezervasyonApi.Data;
using VillaRezervasyonApi.Models;

namespace VillaRezervasyonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillaAvailabilityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VillaAvailabilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/VillaAvailability/5
        [HttpGet("{villaId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetVillaAvailability(int villaId)
        {
            return await _context.Reservations
                .Where(r => r.VillaId == villaId)
                .ToListAsync();
        }

        // POST: api/VillaAvailability
        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateVillaReservation(Reservation reservation)
        {
            // Check if villa exists
            var villa = await _context.Villas.FindAsync(reservation.VillaId);
            if (villa == null)
            {
                return NotFound("Villa not found");
            }

            // Check for overlapping reservations
            var overlappingReservation = await _context.Reservations
                .Where(r => r.VillaId == reservation.VillaId)
                .Where(r => r.StartDate <= reservation.EndDate && r.EndDate >= reservation.StartDate)
                .FirstOrDefaultAsync();

            if (overlappingReservation != null)
            {
                return BadRequest("Villa is already reserved for the selected dates");
            }

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVillaAvailability), new { villaId = reservation.VillaId }, reservation);
        }

        // DELETE: api/VillaAvailability/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVillaReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 