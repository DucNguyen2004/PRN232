using AutoMapper;
using BusinessObjects;
using DTOs;
using Repositories;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<ProductResponseDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<Product> CreateProductAsync(ProductRequestDto dto)
        {
            var convertedProduct = _mapper.Map<Product>(dto);
            return await _productRepository.AddAsync(convertedProduct);
        }

        public async Task UpdateProductAsync(int id, ProductRequestDto dto)
        {
            Product updatedProduct = _mapper.Map<Product>(dto);
            await _productRepository.UpdateAsync(id, updatedProduct);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
