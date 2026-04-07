using ControleGastos.API.Models.DTOs;

namespace ControleGastos.API.Services
{
    // Define as regras de negócio de pessoa
    public interface IPessoaService
    {
        // Retorna todas as pessoas cadastradas
        Task<IEnumerable<PessoaResponseDto>> ListarAsync();

        // Retorna uma pessoa pelo identificador
        Task<PessoaResponseDto?> ObterPorIdAsync(int id);

        // Cria uma nova pessoa
        Task<PessoaResponseDto> CriarAsync(PessoaCreateDto dto);

        // Atualiza uma pessoa existente
        Task<bool> AtualizarAsync(int id, PessoaUpdateDto dto);

        // Remove uma pessoa existente
        Task<bool> RemoverAsync(int id);
    }
}
