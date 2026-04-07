using ControleGastos.API.Models.Entities;

namespace ControleGastos.API.Repositories
{
    // Define as operações de acesso a dados de pessoa
    public interface IPessoaRepository
    {
        Task<IEnumerable<Pessoa>> ListarAsync(); // Retorna todas as pessoas.
        Task<Pessoa?> ObterPorIdAsync(int id); // Retorna uma pessoa pelo identificador.
        Task AdicionarAsync(Pessoa pessoa); // Adiciona uma nova pessoa.
        void Atualizar(Pessoa pessoa); // Atualiza uma pessoa existente.
        void Remover(Pessoa pessoa); // Remove uma pessoa existente.
        Task SalvarAlteracoesAsync(); // Persiste as alterações no banco.
    }
}
