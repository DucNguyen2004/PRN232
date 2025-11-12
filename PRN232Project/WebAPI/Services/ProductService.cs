using BusinessObjects;
using DTOs;
using Repositories;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => Mappers.ProductMapper.ToDTO(p));
        }

        public async Task<ProductResponseDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return Mappers.ProductMapper.ToDTO(product);
        }

        public async Task<Product> CreateProductAsync(ProductRequestDto dto)
        {
            var convertedProduct = Mappers.ProductMapper.ToEntity(dto);
            return await _productRepository.AddAsync(convertedProduct);
        }

        public async Task UpdateProductAsync(int id, ProductRequestDto dto)
        {
            Product updatedProduct = Mappers.ProductMapper.ToEntity(dto);
            await _productRepository.UpdateAsync(id, updatedProduct);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
