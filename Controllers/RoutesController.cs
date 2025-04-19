using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportSystemASP.Models;


namespace TransportSystemASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly TransportSystem2Context _context;

        public RoutesController(TransportSystem2Context context)
        {
            _context = context;
        }

        // GET: api/routes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Route>>> GetRoutes()
        {
            return await _context.Routes.ToListAsync();
        }

        // GET: api/routes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Route>> GetRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);

            if (route == null)
            {
                return NotFound();
            }

            return route;
        }

        // POST: api/routes
        [HttpPost]
        public async Task<ActionResult<Models.Route>> PostRoute(Models.Route route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoute), new { id = route.RouteId }, route);
        }

        // PUT: api/routes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoute(int id, Models.Route route)
        {
            if (id != route.RouteId)
            {
                return BadRequest();
            }

            _context.Entry(route).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Routes.Any(e => e.RouteId == id))
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

        // DELETE: api/routes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
