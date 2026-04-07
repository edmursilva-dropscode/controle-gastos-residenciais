using ControleGastos.API.Models.Enums;

namespace ControleGastos.API.Models.DTOs
{
    // DTO usado para retornar os dados da categoria ao cliente.
    public class CategoriaResponseDto
    {
        public int Id { get; set; } // Identificador da categoria.
        public string Descricao { get; set; } = string.Empty; // Descrição da categoria.
        public FinalidadeCategoria Finalidade { get; set; } // Finalidade da categoria
    }
}
