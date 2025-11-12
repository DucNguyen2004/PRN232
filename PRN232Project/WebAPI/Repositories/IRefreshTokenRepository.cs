using BusinessObjects;

namespace Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetAsync(string token);
        Task SaveAsync(RefreshToken token);
        Task DeleteAsync(RefreshToken token);
    }
}
