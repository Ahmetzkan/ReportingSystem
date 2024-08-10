using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.CrossCuttingConcerns.Transaction
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var actionDescriptor = endpoint?.Metadata.GetMetadata<MethodInfo>();
            if (actionDescriptor != null && actionDescriptor.GetCustomAttributes<TransactionScopeAttribute>().Any())
            {
                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        await _next(context);

                        transactionScope.Complete();
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync($"Transaction failed: {ex.Message}");
                        return;
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
