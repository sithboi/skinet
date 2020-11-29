using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Startup
    {

        //Injected into startup class by app settings or appsettingsdevelopment
        // look at launchsettings.json to deteremine which is being used here
        // launchsettings.json is looked at first by "dotnet run"
        //under that you can see our "API Project" is this json
        //      "environmentVariables": {
        // "ASPNETCORE_ENVIRONMENT": "Development" <---Tells the system to use
        //                                         appsettings.Development.json file
        // }
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;

        }

        //services get injected here
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));
            //lifetimes:
            //scoped = alive for the lifetime of the request
            //transient = only for the life of the method, very short life
            //singleton = never destroyed until application is shut down
            //AddScope(repository to be injected, instance of the concrete class)
            //This allows the ProductRepository to be injectable
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // This is also the middleware area configuration
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // middleware redirector makes it go from http to https
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
