﻿using Business.Dtos.Requests.RefreshTokenRequests;
using Business.Dtos.Requests.UserRequests;
using Business.Dtos.Responses.UserResponses;
using Core.DataAccess.Paging;
using Core.Entities;

namespace Business.Abstracts;

public interface IUserService
{
    Task<IPaginate<GetListUserResponse>> GetListAsync(PageRequest pageRequest);
    Task<CreatedUserResponse> AddAsync(CreateUserRequest createUserRequest);
    Task<UpdatedUserResponse> UpdateAsync(UpdateUserRequest updateUserRequest);
    Task<UpdatedUserResponse> UpdatePasswordAsync(User user,CreateRefreshTokenRequest createRefreshTokenRequest);
    Task<UpdatedUserResponse> UpdateResetTokenAsync(ResetTokenUserRequest resetTokenUserRequest);
    Task<DeletedUserResponse> DeleteAsync(Guid id);
    Task<GetUserResponse> GetByIdAsync(Guid? id);
    Task<GetUserResponse> GetByMailAsync(string email);
    Task<List<OperationClaim>> GetClaimsAsync(User user);
    Task<GetUserResponse> GetByResetTokenAsync(ResetTokenUserRequest resetTokenUserRequest);
}