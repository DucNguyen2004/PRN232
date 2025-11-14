using AutoMapper;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Services;
using System.Security.Claims;

namespace PRN232Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
        {
            var order = _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound("Order not found");

            return Ok(order);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IQueryable<OrderResponseDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders.AsQueryable());
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrdersByUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid or missing user ID in token.");
            }

            var orders = await _orderService.GetAllOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> PlaceOrder([FromBody] OrderRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid order data.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid or missing user ID in token.");
            }

            var order = await _orderService.PlaceOrderAsync(dto, userId);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, _mapper.Map<OrderResponseDto>(order));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string newStatus)
        {
            if (newStatus == null)
            {
                return BadRequest("Invalid status data.");
            }

            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound("Order not found");

            await _orderService.UpdateOrderStatusAsync(id, newStatus);
            return NoContent();
        }

        //[HttpPut("{id:int}")]
        //public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderRequestDto dto)
        //{
        //    if (dto == null)
        //    {
        //        return.BadRequest("Invalid order data.");
        //    }

        //    await _orderService.UpdateOrderAsync(id, dto);
        //    return NoContent();
        //}
    }
}
