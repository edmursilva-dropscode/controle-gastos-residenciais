using ControleGastos.API.Models.Entities;

namespace ControleGastos.API.Repositories
{
    // Define as operações de acesso a dados da entidade Categoria.
    public interface ICategoriaRepository
    {
        Task AdicionarAsync(Categoria categoria); // Adiciona uma nova categoria.
        Task<IEnumerable<Categoria>> ListarAsync(); // Retorna todas as categorias cadastradas.
        Task<Categoria?> ObterPorIdAsync(int id); // Retorna uma categoria pelo identificador.
        Task<bool> PossuiTransacoesVinculadasAsync(int id); // Verifica se a categoria possui transações vinculadas.
        void Remover(Categoria categoria); // Remove uma categoria do contexto.
        Task SalvarAlteracoesAsync(); // Persiste as alterações no banco.
    }
}