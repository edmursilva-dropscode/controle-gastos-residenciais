using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ControleGastos.API.Models.Enums;

namespace ControleGastos.API.Models.Entities
{
    // Representa uma categoria de transação
    public class Categoria
    {
        public int Id { get; set; } // Identificador único da categoria.

        [Required]
        [MaxLength(400)]
        public string Descricao { get; set; } = string.Empty; // Descrição da categoria.

        public FinalidadeCategoria Finalidade { get; set; } // Tipo permitido: despesa, receita ou ambas.

        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>(); // Transações vinculadas à categoria.
    }
}