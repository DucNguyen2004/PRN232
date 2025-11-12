using BusinessObjects;
using DTOs;

namespace Services
{
    public interface IUserService
    {
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<PaginationResponseDto<UserResponseDto>> GetUsersPagedAsync(PaginationRequestDto request);
        Task<User> CreateUserAsync(UserRequestDto createUserDto);
        Task UpdateUserAsync(int id, UserRequestDto updateUserDto);
        Task DeleteUserAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeId);
        Task<bool> IsUsernameUniqueAsync(string username, int? excludeId);
        Task<bool> IsPhoneUniqueAsync(string phone, int? excludeId);
    }
}
