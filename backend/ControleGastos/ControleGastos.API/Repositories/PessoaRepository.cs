using ControleGastos.API.Data;
using ControleGastos.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.API.Repositories
{
    // Implementa o acesso a dados de pessoa
    public class PessoaRepository : IPessoaRepository
    {
        private readonly AppDbContext _context;

        public PessoaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Retorna todas as pessoas cadastradas
        public async Task<IEnumerable<Pessoa>> ListarAsync()
        {
            return await _context.Pessoas
                .AsNoTracking() // Consulta sem rastreamento para melhor performance em leitura.
                .ToListAsync();
        }

        // Retorna uma pessoa pelo identificador
        public async Task<Pessoa?> ObterPorIdAsync(int id)
        {
            return await _context.Pessoas
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Adiciona uma nova pessoa
        public async Task AdicionarAsync(Pessoa pessoa)
        {
            await _context.Pessoas.AddAsync(pessoa);
        }

        // Atualiza uma pessoa existente
        public void Atualizar(Pessoa pessoa)
        {
            _context.Pessoas.Update(pessoa);
        }

        // Remove uma pessoa existente
        public void Remover(Pessoa pessoa)
        {
            _context.Pessoas.Remove(pessoa);
        }

        // Persiste as alterações no banco
        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}