using ControleGastos.API.Data;
using ControleGastos.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.API.Repositories
{
    // Implementa o acesso a dados da entidade Transacao.
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly AppDbContext _context;

        public TransacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Adiciona uma nova transação ao contexto.
        public async Task AdicionarAsync(Transacao transacao)
        {
            await _context.Transacoes.AddAsync(transacao);
        }

        // Retorna todas as transações cadastradas com dados de pessoa e categoria.
        public async Task<IEnumerable<Transacao>> ListarAsync()
        {
            return await _context.Transacoes
                .AsNoTracking() // Consulta sem rastreamento para melhor performance em leitura.
                .Include(t => t.Pessoa) // Inclui os dados da pessoa vinculada.
                .Include(t => t.Categoria) // Inclui os dados da categoria vinculada.
                .OrderBy(t => t.Id) // Ordena pelo identificador.
                .ToListAsync();
        }

        // Retorna uma transação pelo identificador.
        public async Task<Transacao?> ObterPorIdAsync(int id)
        {
            return await _context.Transacoes
                .Include(t => t.Pessoa) // Inclui os dados da pessoa.
                .Include(t => t.Categoria) // Inclui os dados da categoria.
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Remove uma transação do contexto.
        public void Remover(Transacao transacao)
        {
            _context.Transacoes.Remove(transacao);
        }

        // Persiste as alterações no banco de dados.
        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}