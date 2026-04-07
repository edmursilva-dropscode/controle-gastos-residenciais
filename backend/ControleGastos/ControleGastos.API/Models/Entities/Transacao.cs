using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControleGastos.API.Models.Enums;

namespace ControleGastos.API.Models.Entities
{
    // Representa uma transação financeira
    public class Transacao
    {
        public int Id { get; set; }                               // Identificador único da transação.

        [Required]
        [MaxLength(400)]
        public string Descricao { get; set; } = string.Empty;     // Descrição da transação.

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }                        // Valor positivo da transação.

        public TipoTransacao Tipo { get; set; }                   // Tipo da transação: despesa ou receita.

        public int CategoriaId { get; set; }                      // Identificador da categoria.

        public Categoria Categoria { get; set; } = null!;         // Categoria vinculada à transação.

        public int PessoaId { get; set; }                         // Identificador da pessoa.

        public Pessoa Pessoa { get; set; } = null!;               // Pessoa vinculada à transação.
    }
}
