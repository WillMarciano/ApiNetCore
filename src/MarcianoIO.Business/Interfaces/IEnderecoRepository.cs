using MarcianoIO.Business.Models;
using System;
using System.Threading.Tasks;

namespace MarcianoIO.Business.Interface
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
    }
}
