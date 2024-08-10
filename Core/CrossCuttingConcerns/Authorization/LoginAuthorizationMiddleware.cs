using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace Core.CrossCuttingConcerns.Authorization
{
    public class LoginAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (path.StartsWith("/api/Auth/login", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/api/Auth/autoLogin", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/api/Auth/register", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var userRoles = context.User.ClaimRoles();

            if (userRoles == null || !userRoles.Any(role => path.Contains(role, StringComparison.OrdinalIgnoreCase)))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            await _next(context);
        }
    }
}
