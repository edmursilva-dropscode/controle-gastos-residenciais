namespace ControleGastos.API.Models.DTOs
{
    // Dados retornados na consulta de totais por categoria.
    public class RelatorioCategoriasResponseDto
    {
        public IEnumerable<CategoriaResumoDto> Categorias { get; set; } = new List<CategoriaResumoDto>(); // Lista com o resumo por categoria.

        public ResumoGeralDto TotalGeral { get; set; } = new ResumoGeralDto(); // Totais gerais da consulta.
    }
}