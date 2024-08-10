using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dtos.Requests.RefreshTokenRequests
{
    public class RefreshTokenRequest 
    {
        public string RefreshToken { get; set; }
        public string IpAddress { get; set; }
    }
}
