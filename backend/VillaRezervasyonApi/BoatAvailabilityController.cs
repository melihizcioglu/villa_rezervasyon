using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using villa_rezervasyon.Data;
using villa_rezervasyon.Models;

namespace villa_rezervasyon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoatAvailabilityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BoatAvailabilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{boatId}")]
        public async Task<ActionResult<List<BoatReservation>>> GetBoatAvailability(int boatId)
        {
            var reservations = await _context.BoatReservations
                .Where(r => r.BoatId == boatId)
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpPost]
        public async Task<ActionResult<BoatReservation>> CreateBoatReservation(BoatReservation reservation)
        {
            // Check if the boat exists
            var boat = await _context.Boats.FindAsync(reservation.BoatId);
            if (boat == null)
            {
                return NotFound("Boat not found");
            }

            // Check for overlapping reservations
            var overlappingReservations = await _context.BoatReservations
                .Where(r => r.BoatId == reservation.BoatId)
                .Where(r => (r.StartDate <= reservation.EndDate && r.EndDate >= reservation.StartDate))
                .ToListAsync();

            if (overlappingReservations.Any())
            {
                return BadRequest("The selected dates are not available");
            }

            _context.BoatReservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBoatAvailability), new { boatId = reservation.BoatId }, reservation);
        }

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