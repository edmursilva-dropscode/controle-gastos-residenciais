namespace ControleGastos.API.Models.DTOs
{
    // Dados retornados na consulta de totais por pessoa.
    public class RelatorioPessoasResponseDto
    {
        public IEnumerable<PessoaResumoDto> Pessoas { get; set; } = new List<PessoaResumoDto>(); // Lista com o resumo por pessoa.

        public ResumoGeralDto TotalGeral { get; set; } = new ResumoGeralDto(); // Totais gerais da consulta.
    }
}
