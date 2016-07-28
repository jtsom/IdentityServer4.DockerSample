using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace SampleApi
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

 
            string authority = Configuration["IDSRVSAMPLE_ENV_IDSRVHOST"];

            Debug.Assert(!string.IsNullOrEmpty(authority),"Configuration IDSRVSAMPLE_ENV_IDSRVHOST not set");

            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
           {
                Authority = authority,
                ScopeName = "api1",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata=false
            });

            app.UseMvc();
        }

        // Entry point for the application.
        //public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}