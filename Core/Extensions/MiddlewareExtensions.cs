using Core.CrossCuttingConcerns.Authorization;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Logging.SeriLog;
using Core.CrossCuttingConcerns.Transaction;
using Microsoft.AspNetCore.Builder;

namespace Core.Extensions;

public static class MiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        => app.
        UseMiddleware<ExceptionMiddleware>().
        //UseMiddleware<LoginAuthorizationMiddleware>().
        UseMiddleware<AuthorizationMiddleware>().
        UseMiddleware<ValidationMiddleware>().
        UseMiddleware<CacheMiddleware>().
        UseMiddleware<TransactionMiddleware>().
        UseMiddleware<SeriLogMiddleware>();
} 