using ControleGastos.API.Models.DTOs;
using ControleGastos.API.Models.Entities;
using ControleGastos.API.Models.Enums;
using ControleGastos.API.Repositories;

namespace ControleGastos.API.Services
{
    // Implementa as regras de negócio da entidade Categoria.
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        // Cria uma nova categoria com validação de dados.
        public async Task<CategoriaResponseDto> CriarAsync(CategoriaCreateDto dto)
        {
            ValidarCategoria(dto.Descricao, dto.Finalidade);

            var categoria = new Categoria
            {
                Descricao = dto.Descricao.Trim(), // Remove espaços extras para evitar inconsistência no cadastro.
                Finalidade = dto.Finalidade
            };

            await _categoriaRepository.AdicionarAsync(categoria);
            await _categoriaRepository.SalvarAlteracoesAsync(); // Persiste a nova categoria no banco.

            return new CategoriaResponseDto
            {
                Id = categoria.Id,
                Descricao = categoria.Descricao,
                Finalidade = categoria.Finalidade
            };
        }

        // Retorna todas as categorias cadastradas.
        public async Task<IEnumerable<CategoriaResponseDto>> ListarAsync()
        {
            var categorias = await _categoriaRepository.ListarAsync();

            return categorias.Select(c => new CategoriaResponseDto
            {
                Id = c.Id,
                Descricao = c.Descricao,
                Finalidade = c.Finalidade
            });
        }

        // Retorna uma categoria pelo identificador.
        public async Task<CategoriaResponseDto?> ObterPorIdAsync(int id)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(id);

            if (categoria == null)
            {
                return null; // Retorna nulo caso a categoria não exista.
            }

            return new CategoriaResponseDto
            {
                Id = categoria.Id,
                Descricao = categoria.Descricao,
                Finalidade = categoria.Finalidade
            };
        }

        // Exclui uma categoria somente quando ela não possui transações vinculadas.
        public async Task ExcluirAsync(int id)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(id);

            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }

            var possuiTransacoesVinculadas = await _categoriaRepository.PossuiTransacoesVinculadasAsync(id);

            if (possuiTransacoesVinculadas)
            {
                throw new ArgumentException("Não é possível excluir a categoria porque existem transações vinculadas a ela.");
            }

            _categoriaRepository.Remover(categoria);
            await _categoriaRepository.SalvarAlteracoesAsync(); // Persiste a exclusão no banco.
        }

        // Valida os dados da categoria conforme as regras do sistema.
        private static void ValidarCategoria(string descricao, FinalidadeCategoria finalidade)
        {
            if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new ArgumentException("A descrição da categoria é obrigatória."); // Descrição não pode ser vazia.
            }

            if (descricao.Trim().Length > 400)
            {
                throw new ArgumentException("A descrição da categoria deve ter no máximo 400 caracteres."); // Limite definido no enunciado.
            }

            // Garante que apenas os valores previstos no enum sejam aceitos.
            if (!Enum.IsDefined(typeof(FinalidadeCategoria), finalidade))
            {
                throw new ArgumentException("A finalidade informada é inválida. Valores permitidos: Despesa, Receita ou Ambas.");
            }
        }
    }
}