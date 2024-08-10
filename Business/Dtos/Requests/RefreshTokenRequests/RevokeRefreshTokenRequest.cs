using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dtos.Requests.RefreshTokenRequests
{
    public class RevokeRefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string IpAddress { get; set; }
    }
}
