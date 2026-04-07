using ControleGastos.API.Models.DTOs;

namespace ControleGastos.API.Services
{
    // Define as regras de negócio da entidade Transacao.
    public interface ITransacaoService
    {
        // Cria uma nova transação.
        Task<TransacaoResponseDto> CriarAsync(TransacaoCreateDto dto);

        // Retorna todas as transações cadastradas.
        Task<IEnumerable<TransacaoResponseDto>> ListarAsync();

        // Retorna uma transação pelo identificador.
        Task<TransacaoResponseDto?> ObterPorIdAsync(int id);

        // Exclui uma transação pelo identificador.
        Task ExcluirAsync(int id);
    }
}