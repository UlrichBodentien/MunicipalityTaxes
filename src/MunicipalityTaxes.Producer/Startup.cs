using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MunicipalityTaxes.Core.Data;
using MunicipalityTaxes.DataAccess.Model;
using MunicipalityTaxes.DataAccess.Repositories.Tax;
using MunicipalityTaxes.Producer.Exceptions;

namespace MunicipalityTaxes.Producer
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
            services.AddDbContext<MunicipalityContext>(options =>
            {
                var cs = Configuration["Sql:ConnectionString"];
                options.UseSqlServer(cs);
            });

            services.AddTransient<IMunicipalityTaxRepository, MunicipalityTaxRepository>();
            services.AddTransient<ICsvTaxParser, CsvTaxParser>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                await ExceptionHandler.HandleExceptionAsync(context);
            }));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
