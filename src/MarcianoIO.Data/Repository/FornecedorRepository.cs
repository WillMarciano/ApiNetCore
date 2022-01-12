using MarcianoIO.Business.Interfaces;
using MarcianoIO.Business.Models;
using MarcianoIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MarcianoIO.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(BancoContexto contexto) : base(contexto) { }
        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(c => c.Produtos)
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
