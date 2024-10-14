using AutoMapper;
using Business.Abstracts;
using Business.Dtos.Requests.AuthRequests;
using Business.Dtos.Requests.MailRequests;
using Business.Dtos.Requests.OperationClaimRequests;
using Business.Dtos.Requests.RefreshTokenRequests;
using Business.Dtos.Requests.UserOperationClaimRequests;
using Business.Dtos.Requests.UserRequests;
using Business.Dtos.Responses.AuthResponses;
using Business.Dtos.Responses.OperationClaimResponses;
using Business.Dtos.Responses.UserResponses;
using Business.Messages;
using Business.Rules.BusinessRules;
using Core.Entities;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using DataAccess.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Business.Concretes
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IMailService _mailService;
        private readonly IRefreshTokenDal _refreshTokenDal;
        private readonly ITokenHelper _tokenHelper;
        private readonly UserBusinessRules _userBusinessRules;

        public AuthManager(IMapper mapper, UserBusinessRules userBusinessRules,
            IUserOperationClaimService userOperationClaimService, IOperationClaimService operationClaimService, IRefreshTokenDal refreshTokenDal, ITokenHelper tokenHelper, IUserService userService, IRefreshTokenService refreshTokenService, IMailService mailService)
        {
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
            _userOperationClaimService = userOperationClaimService;
            _operationClaimService = operationClaimService;
            _refreshTokenDal = refreshTokenDal;
            _tokenHelper = tokenHelper;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _mailService = mailService;
        }

        public async Task<LoginResponse> Register(RegisterAuthRequest registerAuthRequest, string password, HttpContext httpContext)
        {
            await _userBusinessRules.IsExistsUserMail(registerAuthRequest.Email);
            await _userBusinessRules.VerifyTcKimlikNo(registerAuthRequest);

            User user = _mapper.Map<User>(registerAuthRequest);

            HashingHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            CreateUserRequest createUserRequest = _mapper.Map<CreateUserRequest>(user);

            var addedUser = await _userService.AddAsync(createUserRequest);
            var getUserResponse = await _userService.GetByIdAsync(addedUser.Id);

            User mappedUser = _mapper.Map<User>(getUserResponse);

            GetListOperationClaimResponse operationClaim = await _operationClaimService.GetByRoleName(Roles.User);

            var operationClaimId = operationClaim?.Id ?? (await _operationClaimService.AddAsync(new CreateOperationClaimRequest { Name = "User" })).Id;

            await _userOperationClaimService.AddAsync(new CreateUserOperationClaimRequest
            {
                UserId = addedUser.Id,
                OperationClaimId = operationClaimId
            });

            var ipAddress = GetIpAddress(httpContext);
            return await CreateAccessToken(mappedUser, ipAddress);
        }

        public async Task<User> Login(LoginAuthRequest loginAuthRequest)
        {
            var user = await _userService.GetByMailAsync(loginAuthRequest.Email);
            HashingHelper.VerifyPasswordHash(loginAuthRequest.Password, user.PasswordHash, user.PasswordSalt);

            await _refreshTokenService.DeleteOldRefreshTokens(user.Id);
            return _mapper.Map<User>(user);
        }

        public async Task<LoginResponse> CreateAccessToken(User user, string ipAddress)
        {
            var claims = await _userService.GetClaimsAsync(user);
            var mappedClaims = _mapper.Map<List<OperationClaim>>(claims);

            var existingRefreshToken = await _refreshTokenDal.GetByTokenAsync(user.Id.ToString());
            if (existingRefreshToken != null) { await _refreshTokenDal.DeleteAsync(existingRefreshToken); }

            var accessToken = _tokenHelper.CreateToken(user, mappedClaims);
            var refreshToken = _tokenHelper.GenerateRefreshToken(user.Id, ipAddress);

            await _refreshTokenDal.AddAsync(refreshToken);

            return new LoginResponse
            {
                Token = accessToken.Token,
                RefreshToken = refreshToken.Token,
                Expiration = accessToken.Expiration
            };
        }
        public async Task ChangePassword(ChangePasswordRequest changePasswordRequest, CreateRefreshTokenRequest createRefreshTokenRequest)
        {
            var userResponse = await _userService.GetByIdAsync(changePasswordRequest.UserId);
            HashingHelper.VerifyPasswordHash(changePasswordRequest.OldPassword, userResponse.PasswordHash, userResponse.PasswordSalt);
            HashingHelper.CreatePasswordHash(changePasswordRequest.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            User user = _mapper.Map<User>(userResponse);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Password = changePasswordRequest.NewPassword;

            await _userService.UpdatePasswordAsync(user, createRefreshTokenRequest);
        }

        public async Task ChangeForgotPassword(ResetPasswordRequest resetPasswordRequest, CreateRefreshTokenRequest createRefreshTokenRequest)
        {
            var userResponse = await _userService.GetByIdAsync(resetPasswordRequest.UserId);
            HashingHelper.CreatePasswordHash(resetPasswordRequest.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            User user = _mapper.Map<User>(userResponse);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Password = resetPasswordRequest.NewPassword;

            await _userService.UpdatePasswordAsync(user, createRefreshTokenRequest);
        }

        private string GetIpAddress(HttpContext httpContext)
        {
            string ipAddress = httpContext.Request.Headers.ContainsKey("X-Forwarded-For")
                ? httpContext.Request.Headers["X-Forwarded-For"].ToString()
                : httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()
                    ?? throw new BusinessException(BusinessMessages.InvalidIp);
            return ipAddress;
        }

        public async Task<bool> PasswordResetAsync(string email)
        {
            GetUserResponse user = await _userService.GetByMailAsync(email);

            User mappedUser = _mapper.Map<User>(user);

            var claims = await _userService.GetClaimsAsync(mappedUser);
            var mapped = _mapper.Map<List<OperationClaim>>(claims);
            var resetToken = _tokenHelper.CreateToken(mappedUser, mapped);

            byte[] tokenBytes = Encoding.UTF8.GetBytes(resetToken.Token);
            resetToken.Token = WebEncoders.Base64UrlEncode(tokenBytes);

            SendPasswordResetMailRequest sendPasswordResetMailRequest = new SendPasswordResetMailRequest
            {
                UserId = user.Id,
                ResetToken = resetToken.Token,
                To = email
            };
            ResetTokenUserRequest userPasswordReset = _mapper.Map<ResetTokenUserRequest>(mappedUser);
            userPasswordReset.ResetToken = resetToken.Token;

            await _userService.UpdateResetTokenAsync(userPasswordReset);
            await _mailService.SendPasswordResetMailAsync(sendPasswordResetMailRequest);

            return true;
        }
    }
}