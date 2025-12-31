using Microsoft.AspNetCore.Mvc;
using MS1.OrderService.Models;
using MS1.OrderService.Services;

namespace MS1.OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderManager _service;

    public OrdersController(OrderManager service)
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
