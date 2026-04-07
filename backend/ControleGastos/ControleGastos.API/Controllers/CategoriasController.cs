using ControleGastos.API.Models.DTOs;
using ControleGastos.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.API.Controllers
{
    // Controlador responsável pelos endpoints de categoria.
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // Retorna todas as categorias cadastradas.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaResponseDto>>> Listar()
        {
            var categorias = await _categoriaService.ListarAsync();
            return Ok(categorias);
        }

        // Retorna uma categoria pelo identificador.
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaResponseDto>> ObterPorId(int id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);

            if (categoria == null)
            {
                return NotFound(new { mensagem = "Categoria não encontrada." });
            }

            return Ok(categoria);
        }

        // Cria uma nova categoria.
        [HttpPost]
        public async Task<ActionResult<CategoriaResponseDto>> Criar([FromBody] CategoriaCreateDto dto)
        {
            try
            {
                var categoriaCriada = await _categoriaService.CriarAsync(dto);

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new { id = categoriaCriada.Id },
                    categoriaCriada);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // Exclui uma categoria quando ela não possui transações vinculadas.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                await _categoriaService.ExcluirAsync(id);
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