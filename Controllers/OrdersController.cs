using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportSystemASP.Models;

namespace TransportSystemASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly TransportSystem2Context _context;

        public OrdersController(TransportSystem2Context context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.Driver)
                .Include(o => o.Vehicle)
                .Include(o => o.Route)
                .ToListAsync();
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Driver)
                .Include(o => o.Vehicle)
                .Include(o => o.Route)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            // Проверка разрешения водителя на маршрут
            var permit = await _context.DriverPermits
                .FirstOrDefaultAsync(p => p.DriverId == order.DriverId && p.RouteId == order.RouteId);

            if (permit == null)
            {
                return BadRequest("Водитель не имеет разрешения на выбранный маршрут.");
            }

            // Проверка срока действия разрешения
            if (permit.IssueDate.Date < DateTime.Now)
            {
                return BadRequest("Разрешение водителя на маршрут просрочено.");
            }

            // Проверка грузоподъёмности автомобиля
            var vehicle = await _context.Vehicles.FindAsync(order.VehicleId);
            if (vehicle == null)
            {
                return BadRequest("Автомобиль не найден.");
            }

            if (vehicle.Capacity < order.CargoWeight)
            {
                return BadRequest("Груз превышает допустимую грузоподъёмность автомобиля.");
            }

            // Сохранение заказа
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        // PUT: api/orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(o => o.OrderId == id))
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

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
