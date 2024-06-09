using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Core.Business.Rules;
using Business.Concretes;
using Business.Abstracts;
using Core.Utilities.Security.JWT;


namespace Business
{
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<ITokenHelper, JwtHelper>();
            services.AddScoped<IOperationClaimService, OperationClaimManager>();
            services.AddScoped<IUserOperationClaimService, UserOperationClaimManager>();

            services.AddScoped<IProjectService, ProjectManager>();
            services.AddScoped<ITaskService, TaskManager>();
            services.AddScoped<IReportService, ReportManager>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
            return services;
        }

        public static IServiceCollection AddSubClassesOfType(
        this IServiceCollection services,
        Assembly assembly,
        Type type,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
            foreach (var item in types)
                if (addWithLifeCycle == null)
                    services.AddScoped(item);

                else
                    addWithLifeCycle(services, type);
            return services;
        }
    }
}
