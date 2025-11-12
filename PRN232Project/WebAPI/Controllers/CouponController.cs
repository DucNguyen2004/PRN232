using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace BE_PRN232Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
        }

        [HttpGet("{code}/code")]
        public async Task<ActionResult<CouponResponseDto>> GetCouponByCodeAsync(string code)
        {
            var coupon = await _couponService.GetCouponByCodeAsync(code);
            if (coupon == null)
            {
                return NotFound(new { Message = "Coupon not found." });
            }
            return Ok(coupon);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CouponResponseDto>> GetCouponById(int id)
        {
            var coupon = await _couponService.GetCouponByIdAsync(id);
            if (coupon == null)
            {
                return NotFound(new { Message = "Coupon not found." });
            }
            return Ok(coupon);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponResponseDto>>> GetAllCouponsAsync()
        {
            var coupons = await _couponService.GetAllCouponsAsync();
            return Ok(coupons);
        }

        [HttpPost]
        public async Task<ActionResult<CouponResponseDto>> AddCouponAsync([FromBody] CouponRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Invalid coupon data." });
            }
            var coupon = await _couponService.AddCouponAsync(dto);
            return CreatedAtAction(nameof(GetCouponById), new { id = coupon.Id }, Mappers.CouponMapper.ToDTO(coupon));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCouponAsync(int id, [FromBody] CouponRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Invalid coupon data." });
            }
            try
            {
                await _couponService.UpdateCouponAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Coupon not found." });
            }
        }
    }
}
