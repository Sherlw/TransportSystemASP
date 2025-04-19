using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportSystemASP.Models;

namespace TransportSystemASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverPermitsController : ControllerBase
    {
        private readonly TransportSystem2Context _context;

        public DriverPermitsController(TransportSystem2Context context)
        {
            _context = context;
        }

        // GET: api/driverpermits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverPermit>>> GetDriverPermits()
        {
            return await _context.DriverPermits
                .Include(p => p.Driver)
                .Include(p => p.Route)
                .ToListAsync();
        }

        // GET: api/driverpermits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverPermit>> GetDriverPermit(int id)
        {
            var permit = await _context.DriverPermits
                .Include(p => p.Driver)
                .Include(p => p.Route)
                .FirstOrDefaultAsync(p => p.PermitId == id);

            if (permit == null)
            {
                return NotFound();
            }

            return permit;
        }

        // POST: api/driverpermits
        [HttpPost]
        public async Task<ActionResult<DriverPermit>> PostDriverPermit(DriverPermit permit)
        {
            _context.DriverPermits.Add(permit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDriverPermit), new { id = permit.PermitId }, permit);
        }

        // PUT: api/driverpermits/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverPermit(int id, DriverPermit permit)
        {
            if (id != permit.PermitId)
            {
                return BadRequest();
            }

            _context.Entry(permit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DriverPermits.Any(p => p.PermitId == id))
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

        // DELETE: api/driverpermits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverPermit(int id)
        {
            var permit = await _context.DriverPermits.FindAsync(id);
            if (permit == null)
            {
                return NotFound();
            }

            _context.DriverPermits.Remove(permit);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
