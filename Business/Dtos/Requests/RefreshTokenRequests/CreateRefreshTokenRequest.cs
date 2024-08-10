using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dtos.Requests.RefreshTokenRequests
{
    public class CreateRefreshTokenRequest
    {
        public Guid UserId { get; set; }
        public string IpAddress { get; set; }
    }
}
