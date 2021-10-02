using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Nisshi.Infrastructure
{
    /// <summary>
    /// Pipeline Behavior that implements database transaction functionality
    /// </summary>
    public class ContextTransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly NisshiContext context;

        public ContextTransactionPipelineBehavior(NisshiContext context)
        {
            this.context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = default(TResponse);

            try
            {
                context.BeginTransaction();

                response = await next();

                await context.CommitTransaction();
            }
            catch (Exception)
            {
                context.RollbackTransaction();
                throw;
            }

            return await next();
        }
    }
}