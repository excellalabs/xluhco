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

namespace xluhco.web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

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
            services.AddResponseCaching();

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.Authority = options.Authority + "/v2.0/";

                options.TokenValidationParameters.ValidateIssuer = true; // Enforces that it checks for our specific domain
            });

            services.AddMvc();
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
