using MarcianoIO.Api.Extensions;
using MarcianoIO.Business.Interfaces;
using MarcianoIO.Business.Notificacoes;
using MarcianoIO.Business.Services;
using MarcianoIO.Data.Context;
using MarcianoIO.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MarcianoIO.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<BancoContexto>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IProdutoService, ProdutoService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            return services;
        }
    }
}
