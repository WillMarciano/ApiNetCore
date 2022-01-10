using MarcianoIO.Api.Configuration;
using MarcianoIO.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MarcianoIO.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BancoContexto>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            services.AddIdentityConfiguration(Configuration);
            services.AddAutoMapper(typeof(Startup));            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MarcianoIO.Api", Version = "v1" });
            });
            services.WebApiConfig();
            services.ResolveDependencies();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MarcianoIO.Api v1"));
            }
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseMvcConfiguration();
        }
    }
}
