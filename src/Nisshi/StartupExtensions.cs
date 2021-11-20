using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Nisshi.Infrastructure.Security;
using Nisshi.Models;

namespace Nisshi
{
    /// <summary>
    /// Extension methods for Startup to add JWT authentication
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Injects all authentication services needed to validate and accept JWT tokens
        /// </summary>
        /// <param name="services"></param>
        public static void AddJwt(this IServiceCollection services)
        {
            services.AddOptions();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("wagaNisshiHaSaikouDaYo"));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256); // Not HmacSha512?
            var issuer = "issuer";
            var audience = "audience";

            services.Configure<JwtIssuerOptions>(op =>
            {
                op.Issuer = issuer;
                op.Audience = audience;
                op.SigningCredentials = signingCredentials;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // Ensures that signing keys match
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,
                // Validates issuer claim (iss)
                ValidIssuer = issuer,
                ValidateIssuer = true,
                // Validates audience claim (aud)
                ValidAudience = audience,
                ValidateAudience = true,
                // Validates token expiration
                ValidateLifetime = true,
                // Sets clock drift
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op =>
            {
                op.TokenValidationParameters = tokenValidationParameters;
                op.Events = new JwtBearerEvents
                {
                    // When request received, parse out token from Authorization header
                    OnMessageReceived = (context) =>
                    {
                        var token = context.HttpContext.Request.Headers["Authorization"];

                        if (token.Count > 0 && token[0].StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                            context.Token = token[0].Substring("Token ".Length).Trim();

                        return Task.CompletedTask;
                    }
                };
            });
        }

        /// <summary>
        /// Injects a default implementation of ISwaggerProvider with support for JWT tokens
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(sw =>
            {
                sw.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                sw.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                sw.SwaggerDoc("v1", new OpenApiInfo { Title = "Nisshi API", Version = "v1" });
                sw.CustomSchemaIds(x => x.FullName);
                sw.DocInclusionPredicate((version, apiDescription) => true);

                // TODO Is this needed?
                // sw.TagActionsBy(x => new List<string>()
                // {
                //     x.GroupName ?? throw new InvalidOperationException()
                // });
            });
        }

        /// <summary>
        /// Creates an Entity Data Model for OData
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Entity Data Model</returns>
        public static IEdmModel CreateEdmModel(this IServiceCollection services)
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Aircraft>("Aircraft");
            builder.EntitySet<LogbookEntry>("LogbookEntries");

            return builder.GetEdmModel();
        }
    }
}
