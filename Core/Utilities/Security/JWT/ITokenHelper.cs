using Core.Entities;

namespace Core.Utilities.Security.JWT;

public interface ITokenHelper
{
    AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
    RefreshToken GenerateRefreshToken(Guid userId,string ipAddress);
}