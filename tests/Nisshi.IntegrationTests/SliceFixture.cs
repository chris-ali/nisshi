using System;
using System.IO;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Nisshi.IntegrationTests
{
    /// <summary>
    /// Test fixture that registers all Nisshi services, and then creates a test
    /// database and user accessor. Must be initialized explicitly between tests 
    /// to ensure that a fresh database is created
    /// </summary>
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

            // Replace with in-memory DB for testing
            var db = services.SingleOrDefault(s =>
                s.ServiceType == typeof(DbContextOptions<NisshiContext>));

            if (db != null)
                services.Remove(db);

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

        /// <summary>
        /// Resets the database back to the original in memory state;
        /// call after each test in a test class' Dispose() method
        /// </summary>
        public void ResetDatabase()
        {
            GetNisshiContext().Database.EnsureDeleted();
            GetNisshiContext().Database.EnsureCreated();
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