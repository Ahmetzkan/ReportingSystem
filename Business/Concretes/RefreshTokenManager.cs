using AutoMapper;
using Business.Abstracts;
using Business.Dtos.Requests.RefreshTokenRequests;
using Business.Dtos.Responses.RefreshTokenResponses;
using Business.Messages;
using Business.Rules.BusinessRules;
using Core.Entities;
using Core.Utilities.Security.JWT;
using DataAccess.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class RefreshTokenManager : IRefreshTokenService
{
    private readonly ITokenHelper _tokenHelper;
    private readonly IRefreshTokenDal _refreshTokenDal;
    private readonly TokenOptions _tokenOptions;
    private readonly RefreshTokenBusinessRules _refreshTokenBusinessRules;

    public RefreshTokenManager(
        ITokenHelper tokenHelper,
        IRefreshTokenDal refreshTokenDal,
        RefreshTokenBusinessRules refreshTokenBusinessRules,
        IOptions<TokenOptions> tokenOptions)
    {
        _tokenHelper = tokenHelper;
        _refreshTokenDal = refreshTokenDal;
        _refreshTokenBusinessRules = refreshTokenBusinessRules;
        _tokenOptions = tokenOptions.Value;
    }

    public async Task<CreatedRefreshTokenResponse> CreateRefreshToken(CreateRefreshTokenRequest createRefreshTokenRequest)
    {
        var refreshToken = _tokenHelper.GenerateRefreshToken(createRefreshTokenRequest.UserId, createRefreshTokenRequest.IpAddress);
        await _refreshTokenBusinessRules.RefreshTokenMustBeExists(refreshToken);
        await _refreshTokenBusinessRules.RevokeRefreshTokenShouldBeValid(refreshToken);

        await _refreshTokenDal.AddAsync(refreshToken);
        await DeleteOldRefreshTokens(createRefreshTokenRequest.UserId);

        var user = new User { Id = createRefreshTokenRequest.UserId };
        var accessToken = _tokenHelper.CreateToken(user, new List<OperationClaim>());

        return new CreatedRefreshTokenResponse
        {
            AccessToken = accessToken.Token,
            RefreshToken = refreshToken.Token,
            Expiration = accessToken.Expiration,
        };
    }

    public async Task RevokeRefreshToken(string refreshToken, string ipAddress, string reason, string? replacedByToken = null)
    {
        var token = await _refreshTokenDal.GetByTokenAsync(refreshToken);
        if (token == null) throw new BusinessException(BusinessMessages.InvalidToken);

        await _refreshTokenBusinessRules.RevokeRefreshTokenShouldBeValid(token);

        token.RevokedDate = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;

        await _refreshTokenDal.UpdateAsync(token);
    }

    public async Task<RefreshTokenResponse> RefreshAccessToken(RefreshTokenRequest refreshTokenRequest)
    {
        var existingToken = await _refreshTokenDal.GetByTokenAsync(refreshTokenRequest.RefreshToken);

        if (existingToken == null || !existingToken.IsActive || existingToken.CreatedByIp != refreshTokenRequest.IpAddress) throw new BusinessException(BusinessMessages.InvalidToken);
        if (existingToken.IsExpired) throw new BusinessException(BusinessMessages.ExpiredToken);

        var user = new User { Id = existingToken.UserId };
        var newAccessToken = _tokenHelper.CreateToken(user, new List<OperationClaim>());
        var newRefreshToken = _tokenHelper.GenerateRefreshToken(user.Id, refreshTokenRequest.IpAddress);

        existingToken.RevokedDate = DateTime.UtcNow;
        existingToken.ReplacedByToken = newRefreshToken.Token;
        existingToken.ReasonRevoked = BusinessMessages.GeneratedRefreshToken;

        await _refreshTokenDal.UpdateAsync(existingToken);
        await _refreshTokenDal.AddAsync(newRefreshToken);

        return new RefreshTokenResponse
        {
            AccessToken = newAccessToken.Token,
            RefreshToken = newRefreshToken.Token,
            Expiration = newAccessToken.Expiration
        };
    }

    public async Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken, string ipAddress)
    {
        await _refreshTokenBusinessRules.RotateRefreshTokenShouldBeAllowed(refreshToken);
        RefreshToken newRefreshToken = _tokenHelper.GenerateRefreshToken(user.Id, ipAddress);

        refreshToken.RevokedDate = DateTime.UtcNow;
        refreshToken.ReplacedByToken = newRefreshToken.Token;
        refreshToken.ReasonRevoked = BusinessMessages.ReplacedNewToken;

        await _refreshTokenDal.UpdateAsync(refreshToken);
        await _refreshTokenDal.AddAsync(newRefreshToken);

        return newRefreshToken;
    }

    public async Task RevokeDescendantRefreshTokens(string refreshToken, string ipAddress, string reason)
    {
        var token = await _refreshTokenDal.GetByTokenAsync(refreshToken);
        if (token == null) throw new BusinessException(BusinessMessages.InvalidToken);

        var childToken = await _refreshTokenDal.GetAsync(predicate: r => r.Token == token.ReplacedByToken);

        if (childToken != null && childToken.RevokedDate == null)
        {
            if (!token.IsActive)
            {
                throw new BusinessException(BusinessMessages.InvalidToken);
            }

            await RevokeRefreshToken(childToken.Token, ipAddress, reason, childToken.ReplacedByToken);
            await RevokeDescendantRefreshTokens(childToken.Token, ipAddress, reason);
        }
    }

    public async Task DeleteOldRefreshTokens(Guid userId)
    {
        List<RefreshToken> refreshTokens = await _refreshTokenDal.Query().AsNoTracking()
            .Where(r =>
                    r.UserId == userId
                    && r.RevokedDate == null
                    && r.ExpiresDate >= DateTime.UtcNow
                    && r.CreatedDate.AddDays(_tokenOptions.RefreshTokenExpiration) <= DateTime.UtcNow
            ).ToListAsync();

        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.ReasonRevoked = BusinessMessages.DeletedOldRefreshTokens;
            await _refreshTokenDal.DeleteAsync(refreshToken);
        }
    }
}