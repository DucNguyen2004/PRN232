using BusinessObjects;
using DTOs;

namespace Mappers
{
    public class OrderDetailMapper
    {
        public static OrderDetailResponseDto ToDTO(OrderDetail entity)
        {
            return new OrderDetailResponseDto
            {
                Id = entity.Id,
                Price = entity.Price,
                Quantity = entity.Quantity,
                Product = new ProductOrderDetailResponseDto
                {
                    Id = entity.Product.Id,
                    Name = entity.Product.Name,
                    Price = entity.Product.Price
                },
                ProductOptions = entity.ProductOptions.Select(Mappers.ProductOptionMapper.ToDTO).ToList()
            };
        }

        public static OrderDetail ToEntity(OrderDetailRequestDto dto)
        {
            return new OrderDetail
            {
                Quantity = dto.Quantity,
                Price = dto.Price,
                ProductId = dto.ProductId,
                // ProductOptions and Order will be handled separately
            };
        }
    }
}
