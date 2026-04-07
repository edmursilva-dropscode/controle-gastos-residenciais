namespace ControleGastos.API.Models.DTOs
{
    // Dados do resumo financeiro de uma categoria.
    public class CategoriaResumoDto
    {
        public int CategoriaId { get; set; } // Identificador da categoria.
        public string Descricao { get; set; } = string.Empty; // Descrição da categoria.
        public decimal TotalReceitas { get; set; } // Soma de todas as receitas da categoria.
        public decimal TotalDespesas { get; set; } // Soma de todas as despesas da categoria.
        public decimal Saldo { get; set; } // Saldo da categoria (receitas - despesas).
    }
}