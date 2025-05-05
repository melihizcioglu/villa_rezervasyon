using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VillaRezervasyonApi.Data;
using VillaRezervasyonApi.Models;

namespace VillaRezervasyonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoatAvailabilityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BoatAvailabilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BoatAvailability/5
        [HttpGet("{boatId}")]
        public async Task<ActionResult<IEnumerable<BoatReservation>>> GetBoatAvailability(int boatId)
        {
            return await _context.BoatReservations
                .Where(r => r.BoatId == boatId)
                .ToListAsync();
        }

        // POST: api/BoatAvailability
        [HttpPost]
        public async Task<ActionResult<BoatReservation>> CreateBoatReservation(BoatReservation reservation)
        {
            // Check if boat exists
            var boat = await _context.Boats.FindAsync(reservation.BoatId);
            if (boat == null)
            {
                return NotFound("Boat not found");
            }

            // Check for overlapping reservations
            var overlappingReservation = await _context.BoatReservations
                .Where(r => r.BoatId == reservation.BoatId)
                .Where(r => r.StartDate <= reservation.EndDate && r.EndDate >= reservation.StartDate)
                .FirstOrDefaultAsync();

            if (overlappingReservation != null)
            {
                return BadRequest("Boat is already reserved for the selected dates");
            }

            _context.BoatReservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBoatAvailability), new { boatId = reservation.BoatId }, reservation);
        }

        // DELETE: api/BoatAvailability/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoatReservation(int id)
        {
            var reservation = await _context.BoatReservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.BoatReservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 