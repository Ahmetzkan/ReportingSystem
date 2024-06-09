using DataAccess.Abstracts;
using DataAccess.Concretes;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class DataAccessServiceRegistration
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReportingSystemContext>(options => options.UseSqlServer(configuration.GetConnectionString("ReportingSystemContext")));

            services.AddScoped<IUserDal, EfUserDal>();
            services.AddScoped<IOperationClaimDal, EfOperationClaimDal>();
            services.AddScoped<IUserOperationClaimDal, EfUserOperationClaimDal>();

            services.AddScoped<IProjectDal, EfProjectDal>();
            services.AddScoped<ITaskDal, EfTaskDal>();
            services.AddScoped<IReportDal, EfReportDal>();

            return services;
        }
    }
}
