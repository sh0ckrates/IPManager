using System.Reflection;
using IPManager.Library.Integration.ExternalApi.Abstractions.Configuration;
using IPManager.Library.Integration.ExternalApi.RequestProvider;
using IPManager.Library.Integration.WebApi.Abstractions.RequestProvider;
using IPManager.WebApi.Data.Abstractions.CacheProvider;
using IPManager.WebApi.Data.Abstractions.Configuration;
using IPManager.WebApi.Data.Abstractions.Repositories;
using IPManager.WebApi.Data.CacheProvider;using IPManager.WebApi.Data.DBContext;
using IPManager.WebApi.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using IPManager.WebApi.Core.Abstractions.Providers;
using IPManager.WebApi.Core.Providers;
using Microsoft.EntityFrameworkCore;
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
            var cacheConfig = Configuration
                .GetSection("IPManager:CacheConfig")
                .Get<CacheConfig>();
            var apiConfig = Configuration
                .GetSection("IPManager:WebApiConfig")
                .Get<WebApiConfig>();



            services.AddDbContext<IPManagerDBContext>(cfg =>
            {
                cfg.UseSqlServer(apiConfig.ConnectionString);
            });

            services.AddSingleton(libraryConfig);
            services.AddSingleton(cacheConfig);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddSingleton<ICacheProvider, CacheProvider>();
            services.AddSingleton<IRequestProvider, RequestProvider>();

            services.AddScoped<IIPInfoProvider, IPInfoProvider>();
            services.AddScoped<IIPDetailsRepository, IPDetailsRepository>();

            services.AddSwaggerGen(c => c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "IP Address Manager API", Version = "v1" }));
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
