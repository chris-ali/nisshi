using Nisshi.Infrastructure;

namespace Nisshi.Requests
{
    public class BaseRequest
    {
        protected readonly HeroesDbContext context;

        public BaseRequest(HeroesDbContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();
        }
    }
}