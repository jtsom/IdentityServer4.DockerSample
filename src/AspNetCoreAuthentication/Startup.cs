using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace AspNetCoreAuthentication
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
            services.AddOptions();
            services.Configure<AppOptions>(appOptions =>
            {
                appOptions.ApiUrl = Configuration["IDSRVSAMPLE_ENV_APIURL"];
            });

            services.AddMvc();
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();


            var logger = loggerFactory.CreateLogger("Startup.Configure");

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

 
            string authority = Configuration["IDSRVSAMPLE_ENV_IDSRVHOST"];
            string postLogoutRedirectUri = Configuration["IDSRVSAMPLE_ENV_POSTLOGOUTREDIRECTURI"];

            logger.LogInformation("Config IDSRVSAMPLE_ENV_IDSRVHOST {authority}", authority);
            logger.LogInformation("Config IDSRVSAMPLE_ENV_POSTLOGOUTREDIRECTURI {postLogoutRedirectUri}", postLogoutRedirectUri);


            Debug.Assert(!string.IsNullOrEmpty(authority),"Configuration IDSRVSAMPLE_ENV_IDSRVHOST not set");
            Debug.Assert(!string.IsNullOrEmpty(postLogoutRedirectUri),"Configuration IDSRVSAMPLE_ENV_POSTLOGOUTREDIRECTURI not set");


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true
            });

            
            var oidcOptions = new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",

                Authority = authority,
                RequireHttpsMetadata = false,
                PostLogoutRedirectUri = postLogoutRedirectUri,
                ClientId = "mvc.hybrid",
                ClientSecret = "secret",
                ResponseType = "code id_token",
                GetClaimsFromUserInfoEndpoint = true,
                SaveTokens = true
            };

            oidcOptions.Scope.Clear();
            oidcOptions.Scope.Add("openid");
            oidcOptions.Scope.Add("profile");
            oidcOptions.Scope.Add("api1");
            oidcOptions.Scope.Add("api2");

            app.UseOpenIdConnectAuthentication(oidcOptions);
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}