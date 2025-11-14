using BusinessObjects;
using DTOs;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public JwtService(
            IConfiguration configuration,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LoginResponseDto?> Authenticate(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var allUsers = await _userRepository.GetAllAsync();
            var userAccount = allUsers.FirstOrDefault(u =>
                (u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase) ||
                u.Username.Equals(request.Email, StringComparison.OrdinalIgnoreCase)) &&
                u.Password.Equals(request.Password, StringComparison.OrdinalIgnoreCase));

            if (userAccount == null)
                return null;

            return await GenerateJwtToken(userAccount);
        }

        public async Task<LoginResponseDto?> ValidateRefreshToken(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetAsync(token);
            if (refreshToken is null || refreshToken.Expiry < DateTime.UtcNow)
            {
                return null;
            }

            await _refreshTokenRepository.DeleteAsync(refreshToken);

            var user = await _userRepository.GetByIdAsync(refreshToken.UserId);
            if (user is null) return null;

            return await GenerateJwtToken(user);
        }

        private async Task<LoginResponseDto> GenerateJwtToken(User user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("Jwt:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Roles.ElementAt(0).Name)
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            var refreshTokenString = await GenerateRefreshToken(user.Id);

            return new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                AccessToken = accessToken,
                ExpiresIn = (int)(tokenExpiryTimeStamp - DateTime.UtcNow).TotalSeconds,
                RefreshToken = refreshTokenString
            };
        }

        private async Task<string> GenerateRefreshToken(int userId)
        {
            var refreshTokenValidityMins = _configuration.GetValue<int>("Jwt:RefreshTokenValidityMins");
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                Expiry = DateTime.UtcNow.AddMinutes(refreshTokenValidityMins),
                UserId = userId
            };

            await _refreshTokenRepository.SaveAsync(refreshToken);
            return refreshToken.Token;
        }
    }
}
