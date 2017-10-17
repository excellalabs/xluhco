using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace xluhco.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IShortLinkRepository, CachedShortLinkFromCsvRepository>();
            services.AddSingleton(x => Log.Logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var rewriteOptions = new RewriteOptions()
                // ReSharper disable once ArgumentsStyleLiteral -- kept for readability
                .AddRewrite(@"^(.*)", "/api/Redirect/$1", skipRemainingRules: true);
            app.UseRewriter(rewriteOptions);

            app.UseMvc();
        }
    }
}
