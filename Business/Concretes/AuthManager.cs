using AutoMapper;
using Business.Abstracts;
using Business.Dtos.Requests.AuthRequests;
using Business.Dtos.Requests.OperationClaimRequests;
using Business.Dtos.Requests.UserOperationClaimRequests;
using Business.Dtos.Requests.UserRequests;
using Business.Dtos.Responses.AuthResponses;
using Business.Dtos.Responses.OperationClaimResponses;
using Business.Messages;
using Business.Rules.BusinessRules;
using Core.Entities;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Kps;
using Microsoft.AspNetCore.WebUtilities;
using System.ServiceModel;
using System.Text;

namespace Business.Concretes;

public class AuthManager : IAuthService
{
    private IUserService _userService;
    private ITokenHelper _tokenHelper;
    private IMapper _mapper;
    private IUserOperationClaimService _userOperationClaimService;
    private IOperationClaimService _operationClaimService;
    private UserBusinessRules _userBusinessRules;

    public AuthManager(IUserService userService, ITokenHelper tokenHelper, IMapper mapper, UserBusinessRules userBusinessRules, IUserOperationClaimService userOperationClaimService, IOperationClaimService operationClaimService, KPSPublicSoapClient kPSPublicSoapClient)
    {
        _userService = userService;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
        _userBusinessRules = userBusinessRules;
        _userOperationClaimService = userOperationClaimService;
        _operationClaimService = operationClaimService;
    }

    public async Task<LoginResponse> Register(RegisterAuthRequest registerAuthRequest, string password)
    {
        await _userBusinessRules.IsExistsUserMail(registerAuthRequest.Email);
        await _userBusinessRules.VerifyTcKimlikNo(registerAuthRequest);

        User user = _mapper.Map<User>(registerAuthRequest);

        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        CreateUserRequest createUserRequest = _mapper.Map<CreateUserRequest>(user);

        var addedUser = await _userService.AddAsync(createUserRequest);
        var getUserResponse = await _userService.GetByIdAsync(addedUser.Id);

        User mappedUser = _mapper.Map<User>(getUserResponse);


        GetListOperationClaimResponse operationClaim = await _operationClaimService.GetByRoleName(Roles.User);

        var operationClaimId = operationClaim?.Id ?? (
        await _operationClaimService.AddAsync(new CreateOperationClaimRequest { Name = "User" })).Id;

        await _userOperationClaimService.AddAsync(new CreateUserOperationClaimRequest
        {
            UserId = addedUser.Id,
            OperationClaimId = operationClaimId
        });

        var result = await CreateAccessToken(mappedUser);
        return result;
    }

    public async Task<User> Login(LoginAuthRequest loginAuthRequest)
    {
        var user = await _userService.GetByMailAsync(loginAuthRequest.Email);

        HashingHelper.VerifyPasswordHash(loginAuthRequest.Password, user.PasswordHash, user.PasswordSalt);

        var mappedUser = _mapper.Map<User>(user);
        return mappedUser;
    }

    public async Task<LoginResponse> CreateAccessToken(User user)
    {
        var claims = await _userService.GetClaimsAsync(user);
        var mapped = _mapper.Map<List<OperationClaim>>(claims);

        var accessToken = _tokenHelper.CreateToken(user, mapped);
        LoginResponse loginResponse = _mapper.Map<LoginResponse>(accessToken);

        return loginResponse;
    }

    public async Task ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        byte[] passwordHash, passwordSalt;
        var userResponse = await _userService.GetByIdAsync(changePasswordRequest.UserId);
        HashingHelper.VerifyPasswordHash(changePasswordRequest.OldPassword, userResponse.PasswordHash, userResponse.PasswordSalt);
        HashingHelper.CreatePasswordHash(changePasswordRequest.NewPassword, out passwordHash, out passwordSalt);

        User user = _mapper.Map<User>(userResponse);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.Password = changePasswordRequest.NewPassword;

        await _userService.UpdatePasswordAsync(user);
    }

    public async Task ChangeForgotPassword(ResetPasswordRequest resetPasswordRequest)
    {
        byte[] passwordHash, passwordSalt;
        var userResponse = await _userService.GetByIdAsync(resetPasswordRequest.UserId);
        HashingHelper.CreatePasswordHash(resetPasswordRequest.NewPassword, out passwordHash, out passwordSalt);
        User user = _mapper.Map<User>(userResponse);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.Password = resetPasswordRequest.NewPassword;

        await _userService.UpdatePasswordAsync(user);
    }
}