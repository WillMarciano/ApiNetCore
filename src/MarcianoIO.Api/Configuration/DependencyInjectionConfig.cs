using MarcianoIO.Business.Interface;
using MarcianoIO.Data.Context;
using MarcianoIO.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace MarcianoIO.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<BancoContexto>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            return services;
        }
    }
}
