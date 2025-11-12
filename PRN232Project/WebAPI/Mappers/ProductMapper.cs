using BusinessObjects;
using DTOs;
using Repositories;

namespace Mappers
{
    public class ProductMapper
    {
        private readonly ICategoryRepository _categoryRepository;

        public ProductMapper(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public static ProductResponseDto ToDTO(Product entity)
        {
            return new ProductResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Category = entity.Category != null ? CategoryMapper.ToDTO(entity.Category) : null,
                Description = entity.Description,
                Price = entity.Price,
                CreateAt = entity.CreateAt,
                Sold = entity.Sold,
                Status = entity.Status,
                Images = entity.ProductImages.Select(img => img.Image).ToList(),
                Options = entity.ProductOptions != null ? entity.ProductOptions.Select(ProductOptionMapper.ToDTO).ToList() : new List<ProductOptionResponseDto>()
            };
        }

        public static Product ToEntity(ProductRequestDto dto)
        {

            return new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Description = dto.Description,
                Price = dto.Price,
                CreateAt = dto.CreateAt,
                Sold = 0,
                Status = dto.Status,
                PrevStatus = dto.Status,
                ProductImages = dto.Images.Select(img => new ProductImage { Image = img }).ToList(),
                ProductOptions = dto.Options.Select(ProductOptionMapper.ToEntity).ToList(),
            };
        }
    }
}
