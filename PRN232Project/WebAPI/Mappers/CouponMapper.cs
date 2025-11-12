using BusinessObjects;
using DTOs;

namespace Mappers
{
    public class CouponMapper
    {
        public static CouponResponseDto ToDTO(Coupon entity)
        {
            return new CouponResponseDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                DateCreated = entity.DateCreated,
                DateUpdated = entity.DateUpdated,
                DateValid = entity.DateValid,
                DateExpired = entity.DateExpired,
                MinOrderValue = entity.MinOrderValue,
                ValueType = entity.ValueType,
                Value = entity.Value,
                Product = entity.Product != null ? new ProductCouponResponseDto
                {
                    Id = entity.Product.Id,
                    Name = entity.Product.Name
                } : null,
                Category = entity.Category != null ? CategoryMapper.ToDTO(entity.Category) : null,
                UsageUsers = entity.Usages.Select(u => u.UserId).ToList(),
                MaxUsages = entity.MaxUsages
            };
        }
        public static Coupon ToEntity(CouponRequestDto dto)
        {
            return new Coupon
            {
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                Status = dto.Status,
                DateCreated = dto.DateCreated,
                DateUpdated = DateTime.UtcNow,
                DateValid = dto.DateValid,
                DateExpired = dto.DateExpired,
                MinOrderValue = dto.MinOrderValue,
                ValueType = dto.ValueType,
                Value = dto.Value,
                ProductId = dto.ProductId,
                CategoryId = dto.CategoryId,
                MaxUsages = dto.MaxUsages > 0 ? dto.MaxUsages : 1
            };
        }
    }
}
