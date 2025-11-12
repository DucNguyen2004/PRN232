using BusinessObjects;
using DTOs;

namespace Mappers
{
    public class UserMapper
    {
        public static UserResponseDto ToDTO(User entity)
        {
            return new UserResponseDto
            {
                Id = entity.Id,
                Username = entity.Username,
                Fullname = entity.Fullname,
                Phone = entity.Phone,
                Email = entity.Email,
                Dob = entity.Dob,
                Gender = entity.Gender,
                Status = entity.Status,
                RoleIds = entity.Roles != null ? [.. entity.Roles.Select(role => role.Id)] : [],
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public static User ToEntity(UserRequestDto dto)
        {
            return new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Fullname = dto.Fullname,
                Phone = dto.Phone,
                Email = dto.Email,
                Dob = dto.Dob,
                Gender = dto.Gender,
                Status = dto.Status,
                Roles = dto.RoleIds != null ? [.. dto.RoleIds.Select(roleId => new Role { Id = roleId })] : [],
                CreatedAt = dto.CreatedAt,
                UpdatedAt = DateTime.Now
            };
        }
    }
}
