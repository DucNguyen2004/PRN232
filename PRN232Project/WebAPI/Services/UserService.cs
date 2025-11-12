using BusinessObjects;
using DTOs;
using Repositories;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository;
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return Mappers.UserMapper.ToDTO(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var filteredUsers = users.Where(u => u.Username != "admin");
            return filteredUsers.Select(u => Mappers.UserMapper.ToDTO(u));
        }

        public async Task<PaginationResponseDto<UserResponseDto>> GetUsersPagedAsync(PaginationRequestDto request)
        {
            var (users, total) = await _userRepository.GetPagedAsync(request.Page, request.PageSize);

            return new PaginationResponseDto<UserResponseDto>
            {
                Items = users.Select(Mappers.UserMapper.ToDTO),
                TotalItems = total,
                TotalPages = (int)Math.Ceiling((double)total / request.PageSize),
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<User> CreateUserAsync(UserRequestDto dto)
        {
            var user = Mappers.UserMapper.ToEntity(dto);

            var roles = await _roleRepository.GetAllAsync();
            user.Roles = roles.Where(r => dto.RoleIds.Contains(r.Id)).ToList();

            return await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(int id, UserRequestDto dto)
        {
            User updatedUser = Mappers.UserMapper.ToEntity(dto);

            var roles = await _roleRepository.GetAllAsync();
            updatedUser.Roles = roles.Where(r => dto.RoleIds.Contains(r.Id)).ToList();
            updatedUser.Password = await _userRepository.GetByIdAsync(id).ContinueWith(t => t.Result.Password);

            await _userRepository.UpdateAsync(id, updatedUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null && user.Id != excludeId;
        }

        public async Task<bool> IsUsernameUniqueAsync(string username, int? excludeId)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return user != null && user.Id != excludeId;
        }

        public async Task<bool> IsPhoneUniqueAsync(string phone, int? excludeId)
        {
            var user = await _userRepository.GetByPhoneAsync(phone);
            return user != null && user.Id != excludeId;
        }
    }
}
