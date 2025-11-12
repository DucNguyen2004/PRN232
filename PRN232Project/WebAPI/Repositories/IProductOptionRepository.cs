using BusinessObjects;

namespace Repositories
{
    public interface IProductOptionRepository
    {
        Task<List<ProductOption>> GetAllAsync();
        Task<List<ProductOption>> GetByIdsAsync(List<int> ids, int productId);
    }
}
