using Business.Dtos.Requests.AuthRequests;
using Business.Dtos.Requests.RefreshTokenRequests;
using Business.Dtos.Requests.UserRequests;
using Business.Dtos.Responses.AuthResponses;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Business.Abstracts;

public interface IAuthService
{
    Task<LoginResponse> Register(RegisterAuthRequest registerAuthRequest, string password, HttpContext httpContext);
    Task<User> Login(LoginAuthRequest loginAuthRequest);
    Task<LoginResponse> CreateAccessToken(User user, string ipAddress);
    Task ChangePassword(ChangePasswordRequest changePasswordRequest, CreateRefreshTokenRequest createRefreshTokenRequest);
    Task ChangeForgotPassword(ResetPasswordRequest resetPasswordRequest, CreateRefreshTokenRequest createRefreshTokenRequest);
    Task<bool> PasswordResetAsync(string email);

}