using BusinessObjects;

namespace Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(int id, Product product);
        Task DeleteAsync(int id);
    }
}
