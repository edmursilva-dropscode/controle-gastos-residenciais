using System.ComponentModel.DataAnnotations;
using ControleGastos.API.Models.Enums;

namespace ControleGastos.API.Models.DTOs
{
    // DTO usado para criar uma nova categoria.
    public class CategoriaCreateDto
    {
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [MaxLength(400, ErrorMessage = "A descrição deve ter no máximo 400 caracteres.")]
        public string Descricao { get; set; } = string.Empty; // Descrição da categoria.

        [Required(ErrorMessage = "A finalidade é obrigatória.")]
        public FinalidadeCategoria Finalidade { get; set; } // Finalidade da categoria
    }
}