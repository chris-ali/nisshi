using System.Reflection;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Infrastructure.Security;

namespace Nisshi
{
    public class Startup
    {
        /// <summary>
        /// Path relative from this class to the root of the Angular client project
        /// </summary>
        private readonly string SPA_PATH = "../../ClientApp";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorPipelineBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ContextTransactionPipelineBehavior<,>));

            services.AddControllersWithViews().AddJsonOptions(opt => 
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            services.AddLocalization(x => x.ResourcesPath = "Resources");
            
            services.AddSwaggerService();

            services.AddDbContext<NisshiContext>(opt => 
            { 
                opt.UseInMemoryDatabase("Nisshi"); 
            });

            services.AddCors();
            
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = $"{SPA_PATH}/dist";
            });

            services.AddAutoMapper(GetType().Assembly);

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddJwt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            // Enables middleware to serve required Swagger assets
            app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nisshi API"));

            app.UseRouting();

            app.UseCors(x => 
            {
                x.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
            });

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = $"{SPA_PATH}";

                if (env.IsDevelopment())
                {
                    // Exchange comment tag if you want to run Angular project standalone
                    spa.UseAngularCliServer(npmScript: "start");
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });

            // Also ensures database is seeded if in memory is used
            using (var serviceScope = app.ApplicationServices.CreateScope()) 
            {
                serviceScope.ServiceProvider.GetRequiredService<NisshiContext>().Database.EnsureCreated();
            }
        }
    }
}
