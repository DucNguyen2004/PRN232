namespace Repositories
{
    public interface ICouponUsageRepository
    {
        Task AddUsageAsync(int couponId, int userId);
    }
}
