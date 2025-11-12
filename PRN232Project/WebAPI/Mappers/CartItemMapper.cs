using BusinessObjects;
using DTOs;

namespace Mappers
{
    public class CartItemMapper
    {
        public static CartItemResponseDto ToDTO(CartItem entity)
        {
            return new CartItemResponseDto
            {
                Id = entity.Id,
                Product = new CartItemProductDto
                {
                    Id = entity.Product.Id,
                    Name = entity.Product.Name,
                    Price = entity.Product.Price,
                    Status = entity.Product.Status,
                    Image = entity.Product.ProductImages?.FirstOrDefault()?.Image ?? string.Empty,
                    CategoryId = entity.Product.CategoryId
                },
                ProductOptions = entity.ProductOptions?
                    .Select(ProductOptionMapper.ToDTO)
                    .ToList(),
                Quantity = entity.Quantity
            };
        }

        public static CartItem ToEntity(CartItemRequestDto dto)
        {
            return new CartItem
            {
                Quantity = dto.Quantity,
                ProductId = dto.ProductId,
                ProductOptions = dto.ProductOptionIds?
                    .Select(id => new ProductOption { Id = id })
                    .ToList() ?? new List<ProductOption>()
            };
        }
    }
}
