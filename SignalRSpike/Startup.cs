using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRSpike.Hubs;

namespace SignalRSpike
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
 
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigins",
          builder =>
          {
            builder.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
          });
      });

      services.AddSignalR();
      services.AddMvc();
      services.AddSignalRCore();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
          app.UseDeveloperExceptionPage();
          app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
          {
              HotModuleReplacement = true,
              ReactHotModuleReplacement = true
          });
      }
      else
      {
          app.UseExceptionHandler("/Home/Error");
      }

      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseSignalR(routes =>
      {
        routes.MapHub<EchoHub>("signalr");
      });

      app.UseMvc(routes =>
      {
        routes.MapRoute(
          name: "default",
          template: "{controller=Home}/{action=Index}/{id?}");

        routes.MapSpaFallbackRoute(
          name: "spa-fallback",
          defaults: new { controller = "Home", action = "Index" });
      });
    }
  }
}
