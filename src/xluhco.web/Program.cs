using System;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using xluhco.web.Repositories;

namespace xluhco.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration.MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.Seq("http://localhost:5341")
                    .Enrich.WithProperty("ApplicationName", "xluhco")
                    .Enrich.FromLogContext();
            });

            ConfigureHackyHttpsEnforcement(builder.Services);
            builder.Services.AddResponseCaching();
            builder.Services.AddMemoryCache();
            builder.Services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => builder.Configuration.Bind("AzureAd", options));

            builder.Services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
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

            builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
            builder.Services.AddScoped<LocalCsvShortLinkRepository>();
            builder.Services.Configure<BlobCsvConfiguration>(builder.Configuration.GetSection("BlobCsvStorage"));
            builder.Services.AddScoped<BlobStorageCsvRepository>();
            builder.Services.AddScoped<IShortLinkRepository, CachedShortLinkRepository>((ctx) =>
            {

                var localOrBlob = builder.Configuration["LocalOrBlobStorage"].ToLowerInvariant();

                IShortLinkRepository repoService;
                if (localOrBlob == "blob") { repoService = ctx.GetRequiredService<BlobStorageCsvRepository>(); }
                else { repoService = ctx.GetRequiredService<LocalCsvShortLinkRepository>(); }

                var logger = ctx.GetRequiredService<Serilog.ILogger>();

                return new CachedShortLinkRepository(logger, repoService);
            });
            builder.Services.Configure<RedirectOptions>(builder.Configuration);
            builder.Services.Configure<GoogleAnalyticsOptions>(builder.Configuration);
            builder.Services.Configure<SiteOptions>(builder.Configuration);
            builder.Services.AddApplicationInsightsTelemetry();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
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
                    defaults: new { controller = "Redirect", action = "Index" });
            });

            app.Run();

        }

        /// <summary>
        /// This is a workaround due to issues running in a Linux container with no reverse proxy in front of it.
        /// In this situation, Azure AD authentication attempts to use a redirect URI that is http instead of https.
        /// The out of the box templates for AD + Linux Docker unfortunately don't address this.
        /// For more information: https://github.com/dotnet/aspnetcore/issues/22572
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureHackyHttpsEnforcement(IServiceCollection services)
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

    }
}
