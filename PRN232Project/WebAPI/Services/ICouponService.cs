using BusinessObjects;
using DTOs;

namespace Services
{
    public interface ICouponService
    {
        Task<CouponResponseDto> GetCouponByCodeAsync(string code);
        Task<CouponResponseDto> GetCouponByIdAsync(int id);
        Task<IEnumerable<CouponResponseDto>> GetAllCouponsAsync();
        Task<Coupon> AddCouponAsync(CouponRequestDto dto);
        Task UpdateCouponAsync(int id, CouponRequestDto dto);
    }
}
