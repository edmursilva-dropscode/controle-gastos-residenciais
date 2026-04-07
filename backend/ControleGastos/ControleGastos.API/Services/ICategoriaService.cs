using ControleGastos.API.Models.DTOs;

namespace ControleGastos.API.Services
{
    // Define as regras de negócio da entidade Categoria.
    public interface ICategoriaService
    {
        // Cria uma nova categoria.
        Task<CategoriaResponseDto> CriarAsync(CategoriaCreateDto dto);

        // Retorna todas as categorias cadastradas.
        Task<IEnumerable<CategoriaResponseDto>> ListarAsync();

        // Retorna uma categoria pelo identificador.
        Task<CategoriaResponseDto?> ObterPorIdAsync(int id);

        // Exclui uma categoria, desde que ela não possua transações vinculadas.
        Task ExcluirAsync(int id);
    }
}