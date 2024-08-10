using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RefreshToken : Entity<Guid>
    {

        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresDate { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? ReasonRevoked { get; set; }

        public bool IsActive => RevokedDate == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= ExpiresDate;

        public RefreshToken()
        {
            Token = string.Empty;
            CreatedByIp = string.Empty;
        }

        public RefreshToken(Guid userId, string token, DateTime expiresDate, string createdByIp)
        {
            UserId = userId;
            Token = token;
            ExpiresDate = expiresDate;
            CreatedByIp = createdByIp;
        }

    }
}
