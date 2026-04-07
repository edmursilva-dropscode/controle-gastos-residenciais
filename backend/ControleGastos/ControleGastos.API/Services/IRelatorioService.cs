using ControleGastos.API.Models.DTOs;

namespace ControleGastos.API.Services
{
    // Define as regras de negócio do relatório.
    public interface IRelatorioService
    {
        // Retorna os totais de receitas, despesas e saldo por pessoa.
        Task<RelatorioPessoasResponseDto> ObterTotaisPorPessoaAsync();

        // Retorna os totais de receitas, despesas e saldo por categoria.
        Task<RelatorioCategoriasResponseDto> ObterTotaisPorCategoriaAsync();
    }
}
