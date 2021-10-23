using Nisshi.Infrastructure;

namespace Nisshi.Requests
{
    public class BaseHandler
    {
        protected readonly NisshiContext context;

        public BaseHandler(NisshiContext context)
        {
            this.context = context;
        }
    }
}
