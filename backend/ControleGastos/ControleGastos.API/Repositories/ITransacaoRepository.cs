using ControleGastos.API.Models.Entities;

namespace ControleGastos.API.Repositories
{
    // Define as operações de acesso a dados da entidade Transacao.
    public interface ITransacaoRepository
    {
        Task AdicionarAsync(Transacao transacao); // Adiciona uma nova transação.
        Task<IEnumerable<Transacao>> ListarAsync(); // Retorna todas as transações cadastradas.
        Task<Transacao?> ObterPorIdAsync(int id); // Retorna uma transação pelo identificador.
        void Remover(Transacao transacao); // Remove uma transação do contexto.
        Task SalvarAlteracoesAsync(); // Persiste as alterações no banco.
    }
}