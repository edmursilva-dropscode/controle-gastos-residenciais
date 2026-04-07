namespace ControleGastos.API.Models.DTOs
{
    // Dados do resumo financeiro geral.
    public class ResumoGeralDto
    {
        public decimal TotalReceitas { get; set; } // Soma de todas as receitas.

        public decimal TotalDespesas { get; set; } // Soma de todas as despesas.

        public decimal Saldo { get; set; } // Saldo geral (receitas - despesas).
    }
}
