using System.ComponentModel.DataAnnotations;

namespace ControleGastos.API.Models.DTOs
{
    // Dados para atualizar uma pessoa
    public class PessoaUpdateDto
    {
        [Required(ErrorMessage = "O nome da pessoa é obrigatório.")]
        [MaxLength(200, ErrorMessage = "O nome da pessoa deve ter no máximo 200 caracteres.")]
        public string Nome { get; set; } = string.Empty; // Nome da pessoa.

        [Range(1, 120, ErrorMessage = "A idade informada é inválida.")]
        public int Idade { get; set; } // Idade da pessoa.
    }
}
