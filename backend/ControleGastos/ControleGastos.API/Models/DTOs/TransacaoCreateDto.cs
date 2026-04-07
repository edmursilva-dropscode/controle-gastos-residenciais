using System.ComponentModel.DataAnnotations;
using ControleGastos.API.Models.Enums;

namespace ControleGastos.API.Models.DTOs
{
    // Dados necessários para criar uma nova transação.
    public class TransacaoCreateDto
    {
        [Required(ErrorMessage = "A descrição da transação é obrigatória.")]
        [MaxLength(400, ErrorMessage = "A descrição deve ter no máximo 400 caracteres.")]
        public string Descricao { get; set; } = string.Empty; // Descrição da transação.

        [Required(ErrorMessage = "O valor da transação é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; } // Valor da transação.

        [Required(ErrorMessage = "O tipo da transação é obrigatório.")]
        public TipoTransacao Tipo { get; set; } // Tipo da transação: despesa ou receita.

        [Required(ErrorMessage = "A categoria da transação é obrigatória.")]
        public int CategoriaId { get; set; } // Identificador da categoria vinculada.

        [Required(ErrorMessage = "A pessoa da transação é obrigatória.")]
        public int PessoaId { get; set; } // Identificador da pessoa vinculada.
    }
}