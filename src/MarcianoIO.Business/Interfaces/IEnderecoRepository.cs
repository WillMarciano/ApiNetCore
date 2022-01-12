using MarcianoIO.Business.Models;
using System;
using System.Threading.Tasks;

namespace MarcianoIO.Business.Interfaces
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
    }
}
