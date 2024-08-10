using Business.Dtos.Requests.RefreshTokenRequests;
using Business.Messages;
using Core.Business.Rules;
using Core.Entities;
using DataAccess.Abstracts;
using DataAccess.Concretes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Rules.BusinessRules
{
    public class RefreshTokenBusinessRules : BaseBusinessRules
    {
        private readonly IRefreshTokenDal _refreshTokenDal;

        public RefreshTokenBusinessRules(IRefreshTokenDal refreshTokenDal)
        {
            _refreshTokenDal = refreshTokenDal;
        }

        public async Task RevokeRefreshTokenShouldBeValid(RefreshToken refreshToken)
        {
            if (refreshToken == null || !refreshToken.IsActive)
            {
                throw new BusinessException(BusinessMessages.InvalidToken);
            }
        }


        public async Task RotateRefreshTokenShouldBeAllowed(RefreshToken refreshToken)
        {
            if (refreshToken.RevokedDate != null)
            {
                throw new BusinessException(BusinessMessages.RevokedToken);
            }
        }


        public async Task RefreshTokenMustBeExists(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new BusinessException(BusinessMessages.RefreshTokenIsNull);
        }

    }
}
