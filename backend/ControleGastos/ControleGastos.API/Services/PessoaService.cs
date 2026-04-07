using ControleGastos.API.Models.DTOs;
using ControleGastos.API.Models.Entities;
using ControleGastos.API.Repositories;

namespace ControleGastos.API.Services
{
    // Implementa as regras de negócio de pessoa
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }

        // Retorna todas as pessoas cadastradas
        public async Task<IEnumerable<PessoaResponseDto>> ListarAsync()
        {
            var pessoas = await _pessoaRepository.ListarAsync();

            return pessoas.Select(p => new PessoaResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Idade = p.Idade
            });
        }

        // Retorna uma pessoa pelo identificador
        public async Task<PessoaResponseDto?> ObterPorIdAsync(int id)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(id);

            if (pessoa == null)
            {
                return null; // Retorna nulo caso a pessoa não exista.
            }

            return new PessoaResponseDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Idade = pessoa.Idade
            };
        }

        // Cria uma nova pessoa com validação de dados
        public async Task<PessoaResponseDto> CriarAsync(PessoaCreateDto dto)
        {
            ValidarPessoa(dto.Nome, dto.Idade); // Valida os dados da pessoa antes de criar.

            var pessoa = new Pessoa
            {
                Nome = dto.Nome.Trim(), // Remove espaços desnecessários do nome.
                Idade = dto.Idade
            };

            await _pessoaRepository.AdicionarAsync(pessoa);
            await _pessoaRepository.SalvarAlteracoesAsync(); // Persiste a nova pessoa no banco.

            return new PessoaResponseDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Idade = pessoa.Idade
            };
        }

        // Atualiza os dados de uma pessoa existente
        public async Task<bool> AtualizarAsync(int id, PessoaUpdateDto dto)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(id);

            if (pessoa == null)
            {
                return false; // Retorna falso caso a pessoa não exista.
            }

            ValidarPessoa(dto.Nome, dto.Idade); // Valida os dados antes de atualizar.

            pessoa.Nome = dto.Nome.Trim(); // Atualiza o nome removendo espaços extras.
            pessoa.Idade = dto.Idade;

            _pessoaRepository.Atualizar(pessoa);
            await _pessoaRepository.SalvarAlteracoesAsync(); // Salva as alterações no banco.

            return true;
        }

        // Remove uma pessoa e suas transações associadas
        public async Task<bool> RemoverAsync(int id)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(id);

            if (pessoa == null)
            {
                return false; // Retorna falso caso a pessoa não exista.
            }

            _pessoaRepository.Remover(pessoa);
            await _pessoaRepository.SalvarAlteracoesAsync(); // Remove a pessoa do banco (cascade remove transações).

            return true;
        }

        // Valida os dados da pessoa conforme regras do sistema
        private static void ValidarPessoa(string nome, int idade)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome da pessoa é obrigatório."); // Nome não pode ser vazio.
            }

            if (nome.Trim().Length > 200)
            {
                throw new ArgumentException("O nome da pessoa deve ter no máximo 200 caracteres."); // Limite definido no enunciado.
            }

            if (idade < 1 || idade > 120)
            {
                throw new ArgumentException("A idade informada é inválida."); // Validação de faixa etária.
            }
        }
    }
}