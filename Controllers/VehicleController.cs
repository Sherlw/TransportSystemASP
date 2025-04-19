using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportSystemASP.Models;

namespace TransportSystemASP.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class VehiclesController : ControllerBase
        {
            private readonly TransportSystem2Context _context;

            public VehiclesController(TransportSystem2Context context)
            {
                _context = context;
            }

            // GET: api/vehicles
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
            {
                return await _context.Vehicles.ToListAsync();

               
            }

        
            // GET: api/vehicles/5
            [HttpGet("{id}")]
            public async Task<ActionResult<Vehicle>> GetVehicle(int id)
            {
                var vehicle = await _context.Vehicles.FindAsync(id);

                if (vehicle == null)
                {
                    return NotFound();
                }

                return vehicle;
            }

            // POST: api/vehicles
            [HttpPost]
            public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
            {
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.VehicleId }, vehicle);
            }

            // PUT: api/vehicles/5
            [HttpPut("{id}")]
            public async Task<IActionResult> PutVehicle(int id, Vehicle vehicle)
            {
                if (id != vehicle.VehicleId)
                {
                    return BadRequest();
                }

                _context.Entry(vehicle).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vehicles.Any(e => e.VehicleId == id))
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

            // DELETE: api/vehicles/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteVehicle(int id)
            {
                var vehicle = await _context.Vehicles.FindAsync(id);
                if (vehicle == null)
                {
                    return NotFound();
                }

                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

