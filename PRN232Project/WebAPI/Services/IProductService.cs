using BusinessObjects;
using DTOs;

namespace Services
{
    public interface IProductService
    {
        Task<ProductResponseDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<Product> CreateProductAsync(ProductRequestDto dto);
        Task UpdateProductAsync(int id, ProductRequestDto dto);
        Task DeleteProductAsync(int id);
    }
}
