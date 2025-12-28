using Application.DTOs.Auth;
using Application.DTOs.Auth.Login;
using Application.DTOs.Auth.Password;
using Application.DTOs.Auth.Profile;
using Application.DTOs.Auth.Register;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IConfiguration config, IGenericRepository<User> userRepo, IHttpContextAccessor httpContextAccessor, IGenericRepository<RefreshToken> refreshTokenRepo)
        {
            _config = config;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
            _refreshTokenRepo = refreshTokenRepo;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto input)
        {
            var user = await _userRepo.GetAll()
                  .FirstOrDefaultAsync(u =>
                      u.Email != null &&
                      u.Email.ToLower() == input.Email.Trim().ToLower());


            if (user == null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<User>();
            var passowrdResult = passwordHasher.VerifyHashedPassword(user, user.Password, input.Password);

            if (passowrdResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            await _refreshTokenRepo.Insert(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.UserId,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return new LoginResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto input)
        {
            var existingUser = await _userRepo.GetAll()
                .FirstOrDefaultAsync(u => u.Email!.Trim().ToLower() == input.Email.Trim().ToLower());

            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var passwordHasher = new PasswordHasher<User>();

            var newUser = new User
            {
                FullName = input.FullName,
                Email = input.Email,
                Role = Role.User,
            };

            newUser.Password = passwordHasher.HashPassword(newUser, input.Password);

            await _userRepo.Insert(newUser);
            await _userRepo.SaveChanges(); 

          
            return new RegisterResponseDto
            {
                UserId = newUser.UserId,
                FullName = newUser.FullName,
                Email = newUser.Email,
                Role = newUser.Role,
            };
        }

        public async Task ResetPassword(ResetPasswordDto input)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Convert.ToInt32(userIdClaim);
            var user = await _userRepo.GetById(userId);

            var passwordHasher = new PasswordHasher<User>();
            var passowrdResult = passwordHasher.VerifyHashedPassword(user, user.Password, input.OldPassword);

            if (passowrdResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Old password is incorrect.");
            }

            user.Password = passwordHasher.HashPassword(user, input.NewPassword);
            _userRepo.Update(user);
            await _userRepo.SaveChanges();
        }

        public async Task<UserProfileDto> UserProfile()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Convert.ToInt32(userIdClaim);
            var user = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.UserId == userId && u.Role == Role.User);
            return new UserProfileDto
            {
                FullName = user.FullName,
                Email = user.Email,
                ImageProfile = user.ImageProfile
            };
        }

        public async Task<UserProfileDto> AdminProfile()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Convert.ToInt32(userIdClaim);
            var user = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.UserId == userId && u.Role==Role.Admin);
            return new UserProfileDto
            {
                FullName = user.FullName,
                Email = user.Email,
                ImageProfile = user.ImageProfile
            };
        }

        public async Task<int> GetAllUsers()
        {
            return await _userRepo.GetAll().CountAsync();
        } 

        //=========================================================================================== For Token Generation
        public string GenerateAccessToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = jwtSection["Issuer"],
                Audience = jwtSection["Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var random = new byte[64];
            RandomNumberGenerator.Fill(random);
            return Convert.ToBase64String(random);
        }
        public async Task<string> RefreshToken(string refreshToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = Convert.ToInt32(userIdClaim);

            var storedToken = _refreshTokenRepo.GetAll()
                .FirstOrDefault(rt => rt.UserId == userId && rt.Token == refreshToken && rt.Expires > DateTime.UtcNow);
            if (storedToken == null)
            {
                throw new SecurityTokenException("Invalid refresh token.");
            }
            var user = await _userRepo.GetById(storedToken.UserId);
            return GenerateAccessToken(user);
        }

      
    }
}
