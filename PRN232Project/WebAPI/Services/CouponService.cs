using BusinessObjects;
using DTOs;
using Repositories;

namespace Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
        }

        public async Task<CouponResponseDto> GetCouponByCodeAsync(string code)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            return coupon != null ? Mappers.CouponMapper.ToDTO(coupon) : null;
        }

        public async Task<CouponResponseDto> GetCouponByIdAsync(int id)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);
            return coupon != null ? Mappers.CouponMapper.ToDTO(coupon) : null;
        }

        public async Task<IEnumerable<CouponResponseDto>> GetAllCouponsAsync()
        {
            var coupons = await _couponRepository.GetAllAsync();
            return coupons.Select(Mappers.CouponMapper.ToDTO);
        }

        public async Task<Coupon> AddCouponAsync(CouponRequestDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            dto.DateCreated = DateTime.UtcNow;
            var coupon = Mappers.CouponMapper.ToEntity(dto);
            return await _couponRepository.AddAsync(coupon);
        }

        public async Task UpdateCouponAsync(int id, CouponRequestDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var coupon = Mappers.CouponMapper.ToEntity(dto);
            await _couponRepository.UpdateAsync(id, coupon);
        }
    }
}