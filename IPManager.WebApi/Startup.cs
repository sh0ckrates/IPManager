using IPManager.Library.Integration.ExternalApi.Abstractions.Configuration;
using IPManager.Library.Integration.ExternalApi.Abstractions.ServiceClients;
using IPManager.Library.Integration.ExternalApi.RequestProvider;
using IPManager.Library.Integration.ExternalApi.ServiceClients;
using IPManager.Library.Integration.WebApi.Abstractions.RequestProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace IPManager.WebApi
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
            services.AddControllers();
            services.AddMemoryCache();


            var libraryConfig = Configuration
                .GetSection("IPManager:LibraryConfig")
                .Get<IPManagerConfig>();
            services.AddSingleton(libraryConfig);


            services.AddSingleton<IIPInfoProvider, IPInfoProvider>();
            services.AddSingleton<IRequestProvider, RequestProvider>();

            services.AddSwaggerGen(c => c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "IP Address Processor API", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "IP Address Processor API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
