using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using villa_rezervasyon.Data;
using villa_rezervasyon.Models;

namespace villa_rezervasyon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VillasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Villa>>> GetVillas()
        {
            var villas = await _context.Villas.ToListAsync();
            return Ok(villas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Villa>> GetVilla(int id)
        {
            var villa = await _context.Villas.FindAsync(id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        public async Task<ActionResult<Villa>> CreateVilla(Villa villa)
        {
            _context.Villas.Add(villa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVilla), new { id = villa.Id }, villa);
        }

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