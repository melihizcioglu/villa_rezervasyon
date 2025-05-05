using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VillaRezervasyonApi.Data;
using VillaRezervasyonApi.Models;

namespace VillaRezervasyonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VillasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Villas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Villa>>> GetVillas()
        {
            return await _context.Villas.ToListAsync();
        }

        // GET: api/Villas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Villa>> GetVilla(int id)
        {
            var villa = await _context.Villas.FindAsync(id);

            if (villa == null)
            {
                return NotFound();
            }

            return villa;
        }

        // POST: api/Villas
        [HttpPost]
        public async Task<ActionResult<Villa>> CreateVilla(Villa villa)
        {
            _context.Villas.Add(villa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVilla), new { id = villa.Id }, villa);
        }

        // PUT: api/Villas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVilla(int id, Villa villa)
        {
            if (id != villa.Id)
            {
                return BadRequest();
            }

            _context.Entry(villa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VillaExists(id))
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

        // DELETE: api/Villas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            var villa = await _context.Villas.FindAsync(id);
            if (villa == null)
            {
                return NotFound();
            }

            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VillaExists(int id)
        {
            return _context.Villas.Any(e => e.Id == id);
        }
    }
} 