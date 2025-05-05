using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using villa_rezervasyon.Data;
using villa_rezervasyon.Models;

namespace villa_rezervasyon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAvailabilityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VillaAvailabilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{villaId}")]
        public async Task<ActionResult<List<Reservation>>> GetVillaAvailability(int villaId)
        {
            var reservations = await _context.Reservations
                .Where(r => r.VillaId == villaId)
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateVillaReservation(Reservation reservation)
        {
            // Check if the villa exists
            var villa = await _context.Villas.FindAsync(reservation.VillaId);
            if (villa == null)
            {
                return NotFound("Villa not found");
            }

            // Check for overlapping reservations
            var overlappingReservations = await _context.Reservations
                .Where(r => r.VillaId == reservation.VillaId)
                .Where(r => (r.StartDate <= reservation.EndDate && r.EndDate >= reservation.StartDate))
                .ToListAsync();

            if (overlappingReservations.Any())
            {
                return BadRequest("The selected dates are not available");
            }

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVillaAvailability), new { villaId = reservation.VillaId }, reservation);
        }

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