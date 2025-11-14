using AutoMapper;
using BusinessObjects;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Services;
using System.Security.Claims;

namespace PRN232Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            if (users == null || !users.Any())
            {
                return NotFound("No user found.");
            }

            return Ok(users);
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserResponseDto>> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid or missing user ID in token.");
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid user data.");
            }

            User user = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, _mapper.Map<UserResponseDto>(user));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserRequestDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid user data.");

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userService.UpdateUserAsync(id, dto);
            return NoContent();
        }

        [HttpPut("profile")]
        public async Task<ActionResult> UpdateProfile([FromBody] UserRequestDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid or missing user ID in token.");
            }

            if (dto == null)
                return BadRequest("Invalid user data.");

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userService.UpdateUserAsync(userId, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet("is-existed")]
        public async Task<ActionResult<IsUserExistedDto>> IsUserExisted(
            [FromQuery] string email,
            [FromQuery] string phone,
            [FromQuery] string username = null,
            [FromQuery] int? excludeUserId = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(phone))
            {
                return BadRequest("Phone cannot be empty.");
            }

            IsUserExistedDto res = new IsUserExistedDto
            {
                UserNameExists = !string.IsNullOrWhiteSpace(username)
                    ? await _userService.IsUsernameUniqueAsync(username, excludeUserId)
                    : false,
                EmailExists = await _userService.IsEmailUniqueAsync(email, excludeUserId),
                PhoneExists = await _userService.IsPhoneUniqueAsync(phone, excludeUserId)
            };

            return Ok(res);
        }
    }
}
