using System;
using System.IO;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nisshi.IntegrationTests
{
    public class SliceFixture : IDisposable
    {
        private static readonly IConfiguration config;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ServiceProvider provider;
        private readonly string DatabaseName = $"{Guid.NewGuid()}.db";

        static SliceFixture()
        {
            config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
        }

        public SliceFixture()
        {
            var startup = new Startup(config);
            var services = new ServiceCollection();

            startup.ConfigureServices(services);

            services.AddDbContext<NisshiContext>(opt => opt.UseInMemoryDatabase(DatabaseName));
            services.AddScoped<ICurrentUserAccessor, StubCurrentUserAccessor>();

            provider = services.BuildServiceProvider();

            GetNisshiContext().Database.EnsureCreated();
            scopeFactory = provider.GetService<IServiceScopeFactory>();
        }

        public NisshiContext GetNisshiContext() 
        {
            return provider.GetRequiredService<NisshiContext>();
        }      

        public void Dispose()
        {
            File.Delete(DatabaseName);
        }

        #region Task Executions
        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var context = GetNisshiContext();

                try
                {
                    context.BeginTransaction();

                    await action(scope.ServiceProvider);

                    await context.CommitTransaction();
                }
                catch (Exception)
                {
                    context.RollbackTransaction();
                    throw;
                }
            }
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var context = GetNisshiContext();

                try
                {
                    context.BeginTransaction();

                    var result = await action(scope.ServiceProvider);

                    await context.CommitTransaction();

                    return result;
                }
                catch (Exception)
                {
                    context.RollbackTransaction();
                    throw;
                }
            }
        }

        public Task ExecuteContextAsync(Func<NisshiContext, Task> action)
        {
            return ExecuteScopeAsync(x => action(x.GetService<NisshiContext>()));
        }

        public Task<T> ExecuteContextAsync<T>(Func<NisshiContext, Task<T>> action)
        {
            return ExecuteScopeAsync(x => action(x.GetService<NisshiContext>()));
        }

        public Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(x => 
            {
                var mediator = x.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(x => 
            {
                var mediator = x.GetService<IMediator>();

                return mediator.Send(request);
            });
        }
        #endregion
    }
}