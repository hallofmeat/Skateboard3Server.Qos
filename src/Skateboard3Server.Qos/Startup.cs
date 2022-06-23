using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Autofac;
using JetBrains.Annotations;

namespace Skateboard3Server.Qos;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<QosConfig>(Configuration.GetSection("Qos"));

        services.AddHostedService<QosService>();
        services.AddControllers(options => options.OutputFormatters.Add(new PoxOutputFormatter()));
    }

    [UsedImplicitly]
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new QosRegistry());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}