using Core.DataAccess.Repositories;
using Core.Entities;
using Entities.Concretes;
using System;
using System.Threading.Tasks;

namespace DataAccess.Abstracts
{
    public interface IRefreshTokenDal : IRepository<RefreshToken, Guid>, IAsyncRepository<RefreshToken, Guid>
    {
        Task<RefreshToken> GetByTokenAsync(string token);
    }
}
