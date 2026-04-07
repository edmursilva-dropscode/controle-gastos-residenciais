using ControleGastos.API.Models.DTOs;
using ControleGastos.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoService _transacaoService;

        public TransacoesController(ITransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        // Retorna todas as transações cadastradas.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransacaoResponseDto>>> Listar()
        {
            var transacoes = await _transacaoService.ListarAsync();
            return Ok(transacoes);
        }

        // Retorna uma transação pelo identificador.
        [HttpGet("{id}")]
        public async Task<ActionResult<TransacaoResponseDto>> ObterPorId(int id)
        {
            var transacao = await _transacaoService.ObterPorIdAsync(id);

            if (transacao == null)
            {
                return NotFound(new { mensagem = "Transação não encontrada." });
            }

            return Ok(transacao);
        }

        // Cria uma nova transação aplicando as regras de negócio.
        [HttpPost]
        public async Task<ActionResult<TransacaoResponseDto>> Criar([FromBody] TransacaoCreateDto dto)
        {
            try
            {
                var transacao = await _transacaoService.CriarAsync(dto);

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new { id = transacao.Id },
                    transacao);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // Exclui uma transação pelo identificador.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                await _transacaoService.ExcluirAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}