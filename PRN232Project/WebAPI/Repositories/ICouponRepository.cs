using BusinessObjects;

namespace Repositories
{
    public interface ICouponRepository
    {
        Task<Coupon> GetByCodeAsync(string code);
        Task<Coupon> GetByIdAsync(int id);
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon> AddAsync(Coupon coupon);
        Task UpdateAsync(int id, Coupon coupon);
    }
}
