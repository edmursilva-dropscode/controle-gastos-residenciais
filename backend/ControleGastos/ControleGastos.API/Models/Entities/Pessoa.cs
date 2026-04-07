using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ControleGastos.API.Models.Entities
{
    // Representa uma pessoa do sistema
    public class Pessoa
    {
        public int Id { get; set; }                                                     // Identificador único da pessoa.

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; } = string.Empty;                                // Nome da pessoa.

        public int Idade { get; set; }                                                  // Idade da pessoa.

        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>(); // Transações vinculadas à pessoa.
    }
}
