using ControleGastos.API.Models.DTOs;
using ControleGastos.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.API.Controllers
{
    // Controlador responsável pelos endpoints de pessoa
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;

        public PessoasController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        // Retorna todas as pessoas cadastradas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaResponseDto>>> Listar()
        {
            var pessoas = await _pessoaService.ListarAsync();
            return Ok(pessoas);
        }

        // Retorna uma pessoa pelo identificador
        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaResponseDto>> ObterPorId(int id)
        {
            var pessoa = await _pessoaService.ObterPorIdAsync(id);

            if (pessoa == null)
            {
                return NotFound(new { mensagem = "Pessoa não encontrada." });
            }

            return Ok(pessoa);
        }

        // Cria uma nova pessoa
        [HttpPost]
        public async Task<ActionResult<PessoaResponseDto>> Criar([FromBody] PessoaCreateDto dto)
        {
            try
            {
                var pessoaCriada = await _pessoaService.CriarAsync(dto);

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new { id = pessoaCriada.Id },
                    pessoaCriada);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // Atualiza os dados de uma pessoa existente
        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] PessoaUpdateDto dto)
        {
            try
            {
                var atualizado = await _pessoaService.AtualizarAsync(id, dto);

                if (!atualizado)
                {
                    return NotFound(new { mensagem = "Pessoa não encontrada." });
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // Remove uma pessoa e suas transações associadas
        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            var removido = await _pessoaService.RemoverAsync(id);

            if (!removido)
            {
                return NotFound(new { mensagem = "Pessoa não encontrada." });
            }

            return NoContent();
        }
    }
}