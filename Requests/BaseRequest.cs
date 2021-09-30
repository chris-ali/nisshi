using Nisshi.Infrastructure;

namespace Nisshi.Requests
{
    public class BaseRequest
    {
        protected readonly NisshiContext context;

        public BaseRequest(NisshiContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();
        }
    }
}