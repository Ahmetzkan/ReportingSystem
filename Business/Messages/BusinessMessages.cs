using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Messages
{
    public class BusinessMessages
    {
        public static string DataNotFound = "Data is not found.";
        public static string DataAvailable = "This data is in use.";

        public static string UserNotFound = "User not found.";
        public static string UserLoggedIn = "User logged in.";
        public static string SuccessfulLogin = "Login is succesful.";
        public static string UserAlreadyExists = "User already exists.";
        public static string UserRegistered = "Register is succesful.";
        public static string TcNumberVerifiy = "Tc number authentication failed.";

        public static string InvalidToken = "Invalid token.";
        public static string InvalidIp = "IP address cannot be retrieved from request.";
        public static string InvalidCookie = "Refresh token is not found in request cookies.";
        public static string ExpiredToken = "Token is expired.";

        public static string AuthorizationDenied = "You dont have a authorization.";
        public static string AccessTokenCreated = "Token is created.";
        public static string RefreshTokenIsNull = "Refresh token is null.";

        public static string RevokeReason = "Revoked without replacement.";
        public static string RevokedToken = "Token is revoked.";
        public static string RevokedTokenWithReplacement = "Revoked token without replacement";
        public static string ReplacedNewToken = "Replaced New Token.";

        public static string GeneratedRefreshToken = "Generated refresh token";
        public static string DeletedOldRefreshTokens = "Deleted old refresh tokens";
    }
}
