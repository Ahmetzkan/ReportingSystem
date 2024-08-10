using AutoMapper;
using Business.Dtos.Requests.RefreshTokenRequests;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Profiles
{
    public class RefreshTokenProfile : Profile
    {
        public RefreshTokenProfile()
        {
            CreateMap<CreateRefreshTokenRequest, RefreshToken>();
            CreateMap<RevokeRefreshTokenRequest, RefreshToken>();
            CreateMap<RefreshTokenRequest, RefreshToken>();
        }
    }
}
