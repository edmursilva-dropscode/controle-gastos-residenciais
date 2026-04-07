using ControleGastos.API.Models.DTOs;
using ControleGastos.API.Models.Enums;
using ControleGastos.API.Repositories;

namespace ControleGastos.API.Services
{
    // Implementa as regras de negócio do relatório por pessoa e por categoria.
    public class RelatorioService : IRelatorioService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ITransacaoRepository _transacaoRepository;

        public RelatorioService(
            IPessoaRepository pessoaRepository,
            ICategoriaRepository categoriaRepository,
            ITransacaoRepository transacaoRepository)
        {
            _pessoaRepository = pessoaRepository;
            _categoriaRepository = categoriaRepository;
            _transacaoRepository = transacaoRepository;
        }

        // Retorna os totais de receitas, despesas e saldo por pessoa.
        public async Task<RelatorioPessoasResponseDto> ObterTotaisPorPessoaAsync()
        {
            var pessoas = await _pessoaRepository.ListarAsync();
            var transacoes = await _transacaoRepository.ListarAsync();

            var resumoPessoas = pessoas.Select(pessoa =>
            {
                // Filtra as transações que pertencem à pessoa atual.
                var transacoesDaPessoa = transacoes.Where(t => t.PessoaId == pessoa.Id);

                // Soma apenas as transações do tipo receita.
                var totalReceitas = transacoesDaPessoa
                    .Where(t => t.Tipo == TipoTransacao.Receita)
                    .Sum(t => t.Valor);

                // Soma apenas as transações do tipo despesa.
                var totalDespesas = transacoesDaPessoa
                    .Where(t => t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => t.Valor);

                // Calcula o saldo da pessoa.
                var saldo = totalReceitas - totalDespesas;

                return new PessoaResumoDto
                {
                    PessoaId = pessoa.Id,
                    Nome = pessoa.Nome,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas,
                    Saldo = saldo
                };
            }).ToList();

            // Calcula os totais gerais com base no resumo de cada pessoa.
            var totalReceitasGeral = resumoPessoas.Sum(p => p.TotalReceitas);
            var totalDespesasGeral = resumoPessoas.Sum(p => p.TotalDespesas);

            return new RelatorioPessoasResponseDto
            {
                Pessoas = resumoPessoas,
                TotalGeral = new ResumoGeralDto
                {
                    TotalReceitas = totalReceitasGeral,
                    TotalDespesas = totalDespesasGeral,
                    Saldo = totalReceitasGeral - totalDespesasGeral
                }
            };
        }

        // Retorna os totais de receitas, despesas e saldo por categoria.
        public async Task<RelatorioCategoriasResponseDto> ObterTotaisPorCategoriaAsync()
        {
            var categorias = await _categoriaRepository.ListarAsync();
            var transacoes = await _transacaoRepository.ListarAsync();

            var resumoCategorias = categorias.Select(categoria =>
            {
                // Filtra as transações que pertencem à categoria atual.
                var transacoesDaCategoria = transacoes.Where(t => t.CategoriaId == categoria.Id);

                // Soma apenas as transações do tipo receita.
                var totalReceitas = transacoesDaCategoria
                    .Where(t => t.Tipo == TipoTransacao.Receita)
                    .Sum(t => t.Valor);

                // Soma apenas as transações do tipo despesa.
                var totalDespesas = transacoesDaCategoria
                    .Where(t => t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => t.Valor);

                // Calcula o saldo da categoria.
                var saldo = totalReceitas - totalDespesas;

                return new CategoriaResumoDto
                {
                    CategoriaId = categoria.Id,
                    Descricao = categoria.Descricao,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas,
                    Saldo = saldo
                };
            }).ToList();

            // Calcula os totais gerais com base no resumo de cada categoria.
            var totalReceitasGeral = resumoCategorias.Sum(c => c.TotalReceitas);
            var totalDespesasGeral = resumoCategorias.Sum(c => c.TotalDespesas);

            return new RelatorioCategoriasResponseDto
            {
                Categorias = resumoCategorias,
                TotalGeral = new ResumoGeralDto
                {
                    TotalReceitas = totalReceitasGeral,
                    TotalDespesas = totalDespesasGeral,
                    Saldo = totalReceitasGeral - totalDespesasGeral
                }
            };
        }
    }
}