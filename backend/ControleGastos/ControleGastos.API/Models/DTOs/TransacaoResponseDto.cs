using ControleGastos.API.Models.Enums;

namespace ControleGastos.API.Models.DTOs
{
    // Dados retornados ao consultar uma transação.
    public class TransacaoResponseDto
    {
        public int Id { get; set; } // Identificador da transação.

        public string Descricao { get; set; } = string.Empty; // Descrição da transação.

        public decimal Valor { get; set; } // Valor da transação.

        public TipoTransacao Tipo { get; set; } // Tipo da transação.

        public int CategoriaId { get; set; } // Identificador da categoria.

        public string CategoriaDescricao { get; set; } = string.Empty; // Descrição da categoria.

        public int PessoaId { get; set; } // Identificador da pessoa.

        public string PessoaNome { get; set; } = string.Empty; // Nome da pessoa.
    }
}