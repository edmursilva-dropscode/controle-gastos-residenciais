using ControleGastos.API.Models.DTOs;
using ControleGastos.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.API.Controllers
{
    // Controlador responsável pelos endpoints de relatório.
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly IRelatorioService _relatorioService;

        public RelatoriosController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        // Retorna os totais de receitas, despesas e saldo por pessoa.
        [HttpGet("pessoas")]
        public async Task<ActionResult<RelatorioPessoasResponseDto>> ObterTotaisPorPessoa()
        {
            var relatorio = await _relatorioService.ObterTotaisPorPessoaAsync();
            return Ok(relatorio);
        }

        // Retorna os totais de receitas, despesas e saldo por categoria.
        [HttpGet("categorias")]
        public async Task<ActionResult<RelatorioCategoriasResponseDto>> ObterTotaisPorCategoria()
        {
            var relatorio = await _relatorioService.ObterTotaisPorCategoriaAsync();
            return Ok(relatorio);
        }
    }
}