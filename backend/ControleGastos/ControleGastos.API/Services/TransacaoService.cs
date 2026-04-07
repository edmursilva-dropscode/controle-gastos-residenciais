using ControleGastos.API.Models.DTOs;
using ControleGastos.API.Models.Entities;
using ControleGastos.API.Models.Enums;
using ControleGastos.API.Repositories;

namespace ControleGastos.API.Services
{
    // Implementa as regras de negócio da entidade Transacao.
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public TransacaoService(
            ITransacaoRepository transacaoRepository,
            IPessoaRepository pessoaRepository,
            ICategoriaRepository categoriaRepository)
        {
            _transacaoRepository = transacaoRepository;
            _pessoaRepository = pessoaRepository;
            _categoriaRepository = categoriaRepository;
        }

        // Cria uma nova transação aplicando as validações do sistema.
        public async Task<TransacaoResponseDto> CriarAsync(TransacaoCreateDto dto)
        {
            ValidarTransacao(dto);

            var pessoa = await _pessoaRepository.ObterPorIdAsync(dto.PessoaId);

            if (pessoa == null)
            {
                throw new ArgumentException("Pessoa não encontrada."); // Impede criar transação para pessoa inexistente.
            }

            var categoria = await _categoriaRepository.ObterPorIdAsync(dto.CategoriaId);

            if (categoria == null)
            {
                throw new ArgumentException("Categoria não encontrada."); // Impede criar transação para categoria inexistente.
            }

            // Regra do enunciado:
            // menor de idade só pode ter transação do tipo despesa.
            if (pessoa.Idade < 18 && dto.Tipo == TipoTransacao.Receita)
            {
                throw new ArgumentException("Menor de idade não pode ter receita.");
            }

            // Valida se a finalidade da categoria é compatível com o tipo da transação.
            if (!CategoriaCompativelComTipo(dto.Tipo, categoria.Finalidade))
            {
                throw new ArgumentException("Categoria incompatível com o tipo informado.");
            }

            var transacao = new Transacao
            {
                Descricao = dto.Descricao.Trim(), // Remove espaços extras para evitar inconsistência no cadastro.
                Valor = dto.Valor,
                Tipo = dto.Tipo,
                CategoriaId = dto.CategoriaId,
                PessoaId = dto.PessoaId
            };

            await _transacaoRepository.AdicionarAsync(transacao);
            await _transacaoRepository.SalvarAlteracoesAsync(); // Persiste a nova transação no banco.

            return new TransacaoResponseDto
            {
                Id = transacao.Id,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo,
                CategoriaId = categoria.Id,
                CategoriaDescricao = categoria.Descricao,
                PessoaId = pessoa.Id,
                PessoaNome = pessoa.Nome
            };
        }

        // Retorna todas as transações cadastradas.
        public async Task<IEnumerable<TransacaoResponseDto>> ListarAsync()
        {
            var transacoes = await _transacaoRepository.ListarAsync();

            return transacoes.Select(t => new TransacaoResponseDto
            {
                Id = t.Id,
                Descricao = t.Descricao,
                Valor = t.Valor,
                Tipo = t.Tipo,
                CategoriaId = t.CategoriaId,
                CategoriaDescricao = t.Categoria?.Descricao ?? string.Empty,
                PessoaId = t.PessoaId,
                PessoaNome = t.Pessoa?.Nome ?? string.Empty
            });
        }

        // Retorna uma transação pelo identificador.
        public async Task<TransacaoResponseDto?> ObterPorIdAsync(int id)
        {
            var transacao = await _transacaoRepository.ObterPorIdAsync(id);

            if (transacao == null)
            {
                return null; // Retorna nulo caso a transação não exista.
            }

            return new TransacaoResponseDto
            {
                Id = transacao.Id,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo,
                CategoriaId = transacao.CategoriaId,
                CategoriaDescricao = transacao.Categoria?.Descricao ?? string.Empty,
                PessoaId = transacao.PessoaId,
                PessoaNome = transacao.Pessoa?.Nome ?? string.Empty
            };
        }

        // Exclui uma transação pelo identificador.
        public async Task ExcluirAsync(int id)
        {
            var transacao = await _transacaoRepository.ObterPorIdAsync(id);

            if (transacao == null)
            {
                throw new KeyNotFoundException("Transação não encontrada.");
            }

            _transacaoRepository.Remover(transacao);
            await _transacaoRepository.SalvarAlteracoesAsync(); // Persiste a exclusão no banco.
        }

        // Valida os dados básicos da transação conforme as regras do sistema.
        private static void ValidarTransacao(TransacaoCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Descricao))
            {
                throw new ArgumentException("A descrição da transação é obrigatória."); // Descrição não pode ser vazia.
            }

            if (dto.Descricao.Trim().Length > 400)
            {
                throw new ArgumentException("A descrição da transação deve ter no máximo 400 caracteres."); // Limite definido no enunciado.
            }

            if (dto.Valor <= 0)
            {
                throw new ArgumentException("O valor deve ser maior que zero."); // O enunciado exige valor positivo.
            }

            if (!Enum.IsDefined(typeof(TipoTransacao), dto.Tipo))
            {
                throw new ArgumentException("O tipo da transação informado é inválido."); // Garante valor válido do enum.
            }

            if (dto.PessoaId <= 0)
            {
                throw new ArgumentException("O identificador da pessoa é obrigatório.");
            }

            if (dto.CategoriaId <= 0)
            {
                throw new ArgumentException("O identificador da categoria é obrigatório.");
            }
        }

        // Verifica se a finalidade da categoria permite o tipo da transação informado.
        private static bool CategoriaCompativelComTipo(TipoTransacao tipo, FinalidadeCategoria finalidade)
        {
            if (tipo == TipoTransacao.Despesa)
            {
                return finalidade == FinalidadeCategoria.Despesa || finalidade == FinalidadeCategoria.Ambas;
            }

            if (tipo == TipoTransacao.Receita)
            {
                return finalidade == FinalidadeCategoria.Receita || finalidade == FinalidadeCategoria.Ambas;
            }

            return false;
        }
    }
}