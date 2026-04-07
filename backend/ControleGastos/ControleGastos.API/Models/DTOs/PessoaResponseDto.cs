namespace ControleGastos.API.Models.DTOs
{
    // Dados retornados da pessoa
    public class PessoaResponseDto
    {
        public int Id { get; set; } // Identificador da pessoa.
        public string Nome { get; set; } = string.Empty; // Nome da pessoa.
        public int Idade { get; set; } // Idade da pessoa.
    }
}
