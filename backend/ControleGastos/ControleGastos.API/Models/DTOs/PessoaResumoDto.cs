namespace ControleGastos.API.Models.DTOs
{
    // Dados do resumo financeiro de uma pessoa.
    public class PessoaResumoDto
    {
        public int PessoaId { get; set; } // Identificador da pessoa.
        public string Nome { get; set; } = string.Empty; // Nome da pessoa.
        public decimal TotalReceitas { get; set; } // Soma de todas as receitas da pessoa.
        public decimal TotalDespesas { get; set; } // Soma de todas as despesas da pessoa.
        public decimal Saldo { get; set; } // Saldo da pessoa (receitas - despesas).
    }
}
