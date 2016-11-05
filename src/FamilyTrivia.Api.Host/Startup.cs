using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FamilyTrivia.Contracts;
using FamilyTrivia.Services;
using System.Security.Claims;
using System.Security.Cryptography;
using FamilyTrivia.Contracts.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FamilyTrivia.Api.Host
{
    public class Startup
    {
        Microsoft.IdentityModel.Tokens.SecurityKey _key;
        private TokenAuthOptions _tokenOptions;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {     
            var k = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("TRIVIA!17fcb8d5-26f0-4f32-93de-01ef03d4d7fb"));
            _key = k;

            _tokenOptions = new TokenAuthOptions()
            {
                Audience = "thisSite",
                Issuer = "thisSite",
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)//, SecurityAlgorithms.Sha256Digest SecurityAlgorithms./*, SecurityAlgorithms.Sha256Digest*/)
            };
            services.AddSingleton(_tokenOptions);

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            //services.AddSingleton<IGamesRepositoryService, GamesRepositoryService>();
            //services.AddSingleton<IGamesRepositoryService, MemoryGameRepositoryService>(); // using the memo   
            services.AddSingleton<IClientService, ClientService>();            
            services.AddSingleton<IGamesRepositoryService, AzureRepositoryService>(); // using the memo                        

            services.AddSwaggerGen();
           
            services.AddCors(o => o.AddPolicy("Light", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasId",
                                  policy => policy.RequireClaim(ClaimTypes.Sid));             

            });

            services.AddMvc();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

           

            app.UseSwagger();
            app.UseCors("Light");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                //ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = false,
                //ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,

                RequireExpirationTime = true,
                ValidateLifetime = true,


                ClockSkew = TimeSpan.FromMinutes(2)
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });


            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();

            app.UseMvc();
        }
    }
}
