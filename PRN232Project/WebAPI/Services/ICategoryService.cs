using DTOs;

namespace Services
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
    }
}
