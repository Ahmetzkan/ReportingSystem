using Business.Dtos.Requests.RefreshTokenRequests;
using Business.Dtos.Responses.AuthResponses;
using Business.Dtos.Responses.RefreshTokenResponses;
using Business.Dtos.Responses.TaskResponses;
using Core.DataAccess.Paging;
using Core.Entities;
using System;
using System.Threading.Tasks;

public interface IRefreshTokenService
{
    Task<CreatedRefreshTokenResponse> CreateRefreshToken(CreateRefreshTokenRequest createRefreshTokenRequest);
    Task<RefreshTokenResponse> RefreshAccessToken(RefreshTokenRequest refreshTokenRequest);
    Task RevokeRefreshToken(string refreshToken, string ipAddress, string reason, string? replacedByToken = null);
    Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken, string ipAddress);
    Task RevokeDescendantRefreshTokens(string refreshToken, string ipAddress, string reason);
    Task DeleteOldRefreshTokens(Guid userId);
}
