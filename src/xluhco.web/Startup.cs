using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using xluhco.web.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;

namespace xluhco.web
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public static Serilog.ILogger WireUpLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341")
                .Enrich.WithProperty("ApplicationName", "xluhco")
                .Enrich.FromLogContext()
                .CreateLogger();

            return Log.Logger;

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureHackyHttpsEnforcement(services);
            services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.Authority += "/v2.0/";

                options.TokenValidationParameters.ValidateIssuer = true; // Enforces that it checks for our specific domain
                options.Events = new OpenIdConnectEvents()
                {
                    OnTicketReceived = (context) =>
                    {
                        context.Properties.IsPersistent = true;
                        context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1);

                        return Task.FromResult(0);
                    }
                };
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped(ctx => WireUpLogging());
            services.AddScoped<ShortLinkFromCsvRepository>();
            services.AddScoped<IShortLinkRepository, CachedShortLinkRepository>((ctx) =>
            {
                var repoService = ctx.GetRequiredService<ShortLinkFromCsvRepository>();
                var logger = ctx.GetRequiredService<Serilog.ILogger>();

                return new CachedShortLinkRepository(logger, repoService);
            });
            services.Configure<RedirectOptions>(Configuration);
            services.Configure<GoogleAnalyticsOptions>(Configuration);
            services.Configure<SiteOptions>(Configuration);
            services.AddApplicationInsightsTelemetry();
        }

        /// <summary>
        /// This is a workaround due to issues running in a Linux container with no reverse proxy in front of it.
        /// In this situation, Azure AD authentication attempts to use a redirect URI that is http instead of https.
        /// The out of the box templates for AD + Linux Docker unfortunately don't address this.
        /// For more information: https://github.com/dotnet/aspnetcore/issues/22572
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureHackyHttpsEnforcement(IServiceCollection services)
        {
            // HACK
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                           ForwardedHeaders.XForwardedProto;
                // Only loopback proxies are allowed by default.
                // Clear that restriction because forwarders are enabled by explicit 
                // configuration.
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders();
            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Empty",
                    template: "",
                    defaults: new { controller = "Home", action = "Index" });
                routes.MapRoute(
                    name: "List",
                    template: "list",
                    defaults: new { controller = "Home", action = "List" });
                routes.MapRoute(
                    name: "Default",
                    template: "{shortCode?}",
                    defaults: new {controller = "Redirect", action = "Index"});
            });

        }
    }
}
