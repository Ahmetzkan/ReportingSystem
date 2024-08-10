using AutoMapper;
using Business.Abstracts;
using Business.Dtos.Requests.RefreshTokenRequests;
using Business.Dtos.Requests.UserRequests;
using Business.Dtos.Responses.UserResponses;
using Business.Rules.BusinessRules;
using Core.DataAccess.Paging;
using Core.Entities;
using DataAccess.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Business.Concretes;

public class UserManager : IUserService
{
    private readonly IUserDal _userDal;
    private readonly IOperationClaimService _operationClaimService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IMapper _mapper;
    private readonly UserBusinessRules _userBusinessRules;

    public UserManager(
        IUserDal userDal,
        IMapper mapper,
        UserBusinessRules userBusinessRules,
        IOperationClaimService operationClaimService,
        IRefreshTokenService refreshTokenService)
    {
        _userDal = userDal;
        _mapper = mapper;
        _userBusinessRules = userBusinessRules;
        _operationClaimService = operationClaimService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<CreatedUserResponse> AddAsync(CreateUserRequest createUserRequest)
    {
        var user = _mapper.Map<User>(createUserRequest);
        var addedUser = await _userDal.AddAsync(user);
        return _mapper.Map<CreatedUserResponse>(addedUser);
    }

    public async Task<UpdatedUserResponse> UpdateAsync(UpdateUserRequest updateUserRequest)
    {
        await _userBusinessRules.IsExistsUser(updateUserRequest.Id);

        var user = await _userDal.GetAsync(u => u.Id == updateUserRequest.Id, enableTracking: false);
        var updatedUser = _mapper.Map<User>(updateUserRequest);

        updatedUser.Password = user.Password;
        updatedUser.PasswordSalt = user.PasswordSalt;
        updatedUser.PasswordHash = user.PasswordHash;
        updatedUser.PasswordReset = user.PasswordReset;
        updatedUser.Status = user.Status;

        var result = await _userDal.UpdateAsync(updatedUser);
        return _mapper.Map<UpdatedUserResponse>(result);
    }

    public async Task<UpdatedUserResponse> UpdatePasswordAsync(User user, CreateRefreshTokenRequest createRefreshTokenRequest)
    {
        await _userBusinessRules.IsExistsUser(user.Id);

        var updatedUser = await _userDal.UpdateAsync(user);

        await _refreshTokenService.DeleteOldRefreshTokens(createRefreshTokenRequest.UserId);

        return _mapper.Map<UpdatedUserResponse>(updatedUser);
    }

    public async Task<UpdatedUserResponse> UpdateResetTokenAsync(ResetTokenUserRequest resetTokenUserRequest)
    {
        await _userBusinessRules.IsExistsUser(resetTokenUserRequest.UserId);

        var user = await _userDal.GetAsync(u => u.Id == resetTokenUserRequest.UserId, enableTracking: false);
        user.PasswordReset = resetTokenUserRequest.ResetToken;

        var updatedUser = await _userDal.UpdateAsync(user);
        return _mapper.Map<UpdatedUserResponse>(updatedUser);
    }

    public async Task<DeletedUserResponse> DeleteAsync(Guid id)
    {
        await _userBusinessRules.IsExistsUser(id);

        var user = await _userDal.GetAsync(u => u.Id == id);
        var deletedUser = await _userDal.DeleteAsync(user);

        return _mapper.Map<DeletedUserResponse>(deletedUser);
    }

    public async Task<IPaginate<GetListUserResponse>> GetListAsync(PageRequest pageRequest)
    {
        var userList = await _userDal.GetListAsync(
              include: u => u.Include(u => u.UserOperationClaims)
              .ThenInclude(uopc => uopc.OperationClaim),
              index: pageRequest.PageIndex,
              size: pageRequest.PageSize);
        return _mapper.Map<Paginate<GetListUserResponse>>(userList);
    }

    public async Task<GetUserResponse> GetByIdAsync(Guid? id)
    {
        var user = await _userDal.GetAsync(u => u.Id == id, enableTracking: false);
        return _mapper.Map<GetUserResponse>(user);
    }

    public async Task<GetUserResponse> GetByMailAsync(string email)
    {
        await _userBusinessRules.IsExistsUserByMail(email);

        var user = await _userDal.GetAsync(u => u.Email == email, enableTracking: false);
        return _mapper.Map<GetUserResponse>(user);
    }

    public async Task<GetUserResponse> GetByResetTokenAsync(ResetTokenUserRequest resetTokenUserRequest)
    {
        await _userBusinessRules.IsExistsResetToken(resetTokenUserRequest.ResetToken);

        var user = await _userDal.GetAsync(u => u.PasswordReset == resetTokenUserRequest.ResetToken, enableTracking: false);
        await _refreshTokenService.DeleteOldRefreshTokens(user.Id);

        return _mapper.Map<GetUserResponse>(user);
    }

    public async Task<List<OperationClaim>> GetClaimsAsync(User user)
    {
        var operationClaims = await _operationClaimService.GetByUserIdAsync(user.Id);
        return _mapper.Map<List<OperationClaim>>(operationClaims);
    }
}