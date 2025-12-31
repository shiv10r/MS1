using Microsoft.AspNetCore.Mvc;
using MS1.Models;
using MS1.Services;

namespace MS1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;

        public OrdersController(OrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllOrders());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = _service.GetOrderById(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Order order)
        {
            try
            {
                var created = _service.CreateOrder(order);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
