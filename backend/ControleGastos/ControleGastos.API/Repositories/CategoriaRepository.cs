using ControleGastos.API.Data;
using ControleGastos.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.API.Repositories
{
    // Implementa o acesso a dados da entidade Categoria.
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Adiciona uma nova categoria ao contexto.
        public async Task AdicionarAsync(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
        }

        // Retorna todas as categorias cadastradas ordenadas pela descrição.
        public async Task<IEnumerable<Categoria>> ListarAsync()
        {
            return await _context.Categorias
                .AsNoTracking() // Consulta sem rastreamento para melhor performance em leitura.
                .OrderBy(c => c.Descricao)
                .ToListAsync();
        }

        // Retorna uma categoria pelo identificador.
        public async Task<Categoria?> ObterPorIdAsync(int id)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Verifica se existe ao menos uma transação vinculada à categoria.
        public async Task<bool> PossuiTransacoesVinculadasAsync(int id)
        {
            return await _context.Transacoes
                .AnyAsync(t => t.CategoriaId == id);
        }

        // Remove uma categoria do contexto.
        public void Remover(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
        }

        // Persiste as alterações no banco.
        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}