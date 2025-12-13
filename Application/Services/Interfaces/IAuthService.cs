using Application.DTOs.Auth;
using Application.DTOs.Auth.Login;
using Application.DTOs.Auth.Password;
using Application.DTOs.Auth.Profile;
using Application.DTOs.Auth.Register;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto input);
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto input);
        Task<UserProfileDto> UserProfile();
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        Task ResetPassword(ResetPasswordDto input);
        Task<string> RefreshToken(string refreshToken);
    }
}
