using Core.CrossCuttingConcerns.Logging.SeriLog.Logger;
using Core.CrossCuttingConcerns.Logging;
using Core.DataAccess.Repositories;
using Core.Entities;
using DataAccess.Abstracts;
using DataAccess.Contexts;
using Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DataAccess.Concretes
{
    public class EfRefreshTokenDal : EfRepositoryBase<RefreshToken, Guid, ReportingSystemContext>, IRefreshTokenDal
    {
        public EfRefreshTokenDal(ReportingSystemContext context) : base(context)
        {
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            return await Context.Set<RefreshToken>().SingleOrDefaultAsync(rt => rt.Token == token);
        }
    }
}
