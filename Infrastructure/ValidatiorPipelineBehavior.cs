using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Nisshi.Infrastructure
{
    /// <summary>
    /// Pipeline Behavior that intercepts requests and validates their 
    /// associated entities using FluentValidation
    /// </summary>
    public class ValidatiorPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidatiorPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = validators
                .Select(x => x.Validate(context))
                .SelectMany(y => y.Errors)
                .Where(z => z != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}