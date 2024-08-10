using Business.Abstracts;
using Business.Dtos.Requests.AuthRequests;
using Business.Dtos.Requests.RefreshTokenRequests;
using Business.Dtos.Requests.UserRequests;
using Business.Dtos.Responses.AuthResponses;
using Business.Dtos.Responses.RefreshTokenResponses;
using Business.Messages;
using Core.Entities;
using DataAccess.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(IAuthService authService, IRefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("autoLogin")]
        public async Task<IActionResult> AutoLogin()
        {
            var ipAddress = GetIpAddress();
            LoginAuthRequest loginAuthRequest = new LoginAuthRequest
            {
                Email = "ahmet@ahmet.com",
                Password = "ahmet"
            };
            var userToLogin = await _authService.Login(loginAuthRequest);
            var result = await _authService.CreateAccessToken(userToLogin, ipAddress);
            if (result.RefreshToken is not null) SetRefreshTokenToCookie(result.RefreshToken, result.Expiration);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginAuthRequest loginAuthRequest)
        {
            var ipAddress = GetIpAddress();

            var userToLogin = await _authService.Login(loginAuthRequest);
            var result = await _authService.CreateAccessToken(userToLogin, ipAddress);
            if (result.RefreshToken is not null) SetRefreshTokenToCookie(result.RefreshToken, result.Expiration);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterAuthRequest registerAuthRequest)
        {
            var ipAddress = GetIpAddress();
            var registerResult = await _authService.Register(registerAuthRequest, registerAuthRequest.Password, HttpContext);
            return Ok(registerResult);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var createRefreshTokenRequest = new CreateRefreshTokenRequest { IpAddress = GetIpAddress() };
            await _authService.ChangePassword(changePasswordRequest, createRefreshTokenRequest);
            return Ok();
        }

        [HttpPost("change-forgot-password")]
        public async Task<IActionResult> ChangeForgotPasswordAsync([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var createRefreshTokenRequest = new CreateRefreshTokenRequest { IpAddress = GetIpAddress() };
            await _authService.ChangeForgotPassword(resetPasswordRequest, createRefreshTokenRequest);
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenResponse>> Refresh()
        {
            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = GetRefreshTokenFromCookies(),
                IpAddress = GetIpAddress()
            };

            var response = await _refreshTokenService.RefreshAccessToken(refreshTokenRequest);
            SetRefreshTokenToCookie(response.RefreshToken, response.Expiration);
            return Ok(response);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RevokeRefreshTokenRequest revokeRefreshTokenRequest)
        {
            var ipAddress = GetIpAddress();
            await _refreshTokenService.RevokeDescendantRefreshTokens(revokeRefreshTokenRequest.RefreshToken, ipAddress, BusinessMessages.RevokeReason);
            await _refreshTokenService.RevokeRefreshToken(revokeRefreshTokenRequest.RefreshToken, ipAddress, BusinessMessages.RevokeReason);
            return NoContent();
        }

        private string GetRefreshTokenFromCookies()
        {
            return HttpContext.Request.Cookies["RefreshToken"] ?? throw new ArgumentException(BusinessMessages.InvalidCookie);
        }

        protected string GetIpAddress()
        {
            string ipAddress = Request.Headers.ContainsKey("X-Forwarded-For")
                ? Request.Headers["X-Forwarded-For"].ToString()
                : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()
                    ?? throw new InvalidOperationException(BusinessMessages.InvalidIp);
            return ipAddress;
        }

        private void SetRefreshTokenToCookie(string refreshToken, DateTime expiresDate)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expiresDate,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };
            Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
        }
    }
}