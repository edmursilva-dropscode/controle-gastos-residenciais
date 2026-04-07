import { useEffect, useState } from "react";
import { api } from "../services/api";
import { Pessoa } from "../types/Pessoa";
import "../App.css";

// Estrutura da resposta de validação retornada pela API
type RespostaValidacao = {
  errors?: Record<string, string[]>;
  message?: string;
  mensagem?: string;
};

// Estrutura do erro retornado pelo axios com possível resposta da API
type ErroComResposta = {
  response?: {
    data?: RespostaValidacao | string;
  };
};

// Página responsável pelo cadastro, edição, exclusão e listagem de pessoas
export function PessoasPage() {
  // Lista de pessoas carregadas da API
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);

  // Nome informado no formulário
  const [nome, setNome] = useState("");

  // Idade informada no formulário
  const [idade, setIdade] = useState("");

  // Controla se está editando uma pessoa (null = criação)
  const [idEmEdicao, setIdEmEdicao] = useState<number | null>(null);

  // Página atual da listagem
  const [paginaAtual, setPaginaAtual] = useState(1);

  // Quantidade de itens por página
  const itensPorPagina = 10;

  // Carrega as pessoas ao abrir a tela
  useEffect(() => {
    carregarPessoas();
  }, []);

  // Mantém a página atual sincronizada com a quantidade de páginas existente.
  // Isso evita ficar preso em uma página inexistente após exclusões.
  useEffect(() => {
    const totalPaginasCalculado = Math.max(
      1,
      Math.ceil(pessoas.length / itensPorPagina)
    );

    if (paginaAtual > totalPaginasCalculado) {
      setPaginaAtual(totalPaginasCalculado);
    }
  }, [pessoas, paginaAtual]);

  // Busca todas as pessoas cadastradas no back-end
  async function carregarPessoas() {
    try {
      const response = await api.get("/pessoas");
      setPessoas(response.data);
    } catch (error) {
      console.error("Erro ao buscar pessoas", error);
      alert("Não foi possível carregar a lista de pessoas.");
    }
  }

  // Limpa os campos do formulário e cancela edição
  function limparFormulario() {
    setNome("");
    setIdade("");
    setIdEmEdicao(null);
  }

  // Preenche o formulário para edição de uma pessoa existente
  function iniciarEdicao(pessoa: Pessoa) {
    setNome(pessoa.nome);
    setIdade(String(pessoa.idade));
    setIdEmEdicao(pessoa.id);
  }

  // Extrai mensagens de validação vindas da API
  function obterMensagensDeValidacao(
    resposta: RespostaValidacao | string | undefined
  ): string | null {
    if (!resposta) {
      return null;
    }

    if (typeof resposta === "string") {
      return resposta;
    }

    if (resposta.errors) {
      const mensagens = Object.values(resposta.errors).flat().join("\n");
      return mensagens || null;
    }

    if (resposta.message) {
      return resposta.message;
    }

    if (resposta.mensagem) {
      return resposta.mensagem;
    }

    return null;
  }

  // Cria ou atualiza uma pessoa
  async function salvarPessoa() {
    try {
      // Validação de nome
      if (!nome.trim()) {
        alert("O nome da pessoa é obrigatório.");
        return;
      }

      if (nome.trim().length > 200) {
        alert("O nome da pessoa deve ter no máximo 200 caracteres.");
        return;
      }

      // Validação de idade
      if (!idade || Number(idade) < 1 || Number(idade) > 120) {
        alert("A idade informada é inválida.");
        return;
      }

      // Dados enviados para a API
      const payload = {
        nome: nome.trim(),
        idade: Number(idade),
      };

      // Decide entre criação ou atualização
      if (idEmEdicao === null) {
        await api.post("/pessoas", payload);
      } else {
        await api.put(`/pessoas/${idEmEdicao}`, payload);
      }

      limparFormulario();
      await carregarPessoas();
    } catch (error: unknown) {
      console.error("Erro ao salvar pessoa", error);

      const erroComResposta = error as ErroComResposta;

      // Tenta obter mensagens amigáveis da API
      const mensagensValidacao = obterMensagensDeValidacao(
        erroComResposta.response?.data
      );

      if (mensagensValidacao) {
        alert(mensagensValidacao);
        return;
      }

      alert("Não foi possível salvar os dados da pessoa.");
    }
  }

  // Remove uma pessoa da base de dados
  async function excluirPessoa(id: number, nomePessoa: string) {
    const confirmar = window.confirm(
      `Deseja realmente excluir a pessoa "${nomePessoa}"?`
    );

    if (!confirmar) {
      return;
    }

    try {
      await api.delete(`/pessoas/${id}`);

      if (idEmEdicao === id) {
        limparFormulario();
      }

      await carregarPessoas();
    } catch (error: unknown) {
      console.error("Erro ao excluir pessoa", error);

      const erroComResposta = error as ErroComResposta;

      // Tenta obter mensagens amigáveis da API
      const mensagensValidacao = obterMensagensDeValidacao(
        erroComResposta.response?.data
      );

      if (mensagensValidacao) {
        alert(mensagensValidacao);
        return;
      }

      alert("Não foi possível excluir a pessoa.");
    }
  }

  // Total de páginas disponíveis.
  // Usa Math.max para garantir pelo menos 1 página lógica.
  const totalPaginas = Math.max(1, Math.ceil(pessoas.length / itensPorPagina));

  // Ajusta a página usada na renderização para nunca ultrapassar o total disponível.
  const paginaAtualAjustada = Math.min(paginaAtual, totalPaginas);

  // Calcula os índices da paginação com base na página ajustada
  const indiceInicial = (paginaAtualAjustada - 1) * itensPorPagina;
  const indiceFinal = indiceInicial + itensPorPagina;

  // Lista de pessoas exibidas na página atual
  const pessoasPaginadas = pessoas.slice(indiceInicial, indiceFinal);

  // Vai para a página anterior
  function paginaAnterior() {
    if (paginaAtualAjustada > 1) {
      setPaginaAtual(paginaAtualAjustada - 1);
    }
  }

  // Vai para a próxima página
  function proximaPagina() {
    if (paginaAtualAjustada < totalPaginas) {
      setPaginaAtual(paginaAtualAjustada + 1);
    }
  }

  // Renderização da tela
  return (
    <div className="page">
      <div className="card">
        <h1 className="page-title">
          {idEmEdicao === null ? "Cadastro de Pessoas" : "Editando Pessoa"}
        </h1>

        <div className="form-row">
          <input
            className="input"
            type="text"
            placeholder="Nome"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
          />

          <input
            className="input"
            type="number"
            placeholder="Idade"
            value={idade}
            onChange={(e) => setIdade(e.target.value)}
          />

          <button className="button" onClick={salvarPessoa}>
            {idEmEdicao === null ? "Cadastrar" : "Salvar"}
          </button>

          {idEmEdicao !== null && (
            <button
              className="button button-secondary"
              onClick={limparFormulario}
            >
              Cancelar
            </button>
          )}
        </div>

        <ul className="people-list">
          {pessoasPaginadas.map((p) => (
            <li key={p.id} className="person-item">
              <span>
                {p.nome} - {p.idade} anos
              </span>

              <div className="actions">
                <button
                  className="button button-edit"
                  onClick={() => iniciarEdicao(p)}
                >
                  Editar
                </button>

                <button
                  className="button button-danger"
                  onClick={() => excluirPessoa(p.id, p.nome)}
                >
                  Excluir
                </button>
              </div>
            </li>
          ))}
        </ul>

        {totalPaginas > 1 && (
          <div className="pagination">
            <button
              className="button"
              onClick={paginaAnterior}
              disabled={paginaAtualAjustada === 1}
            >
              Anterior
            </button>

            <span className="pagination-info">
              Página {paginaAtualAjustada} de {totalPaginas}
            </span>

            <button
              className="button"
              onClick={proximaPagina}
              disabled={paginaAtualAjustada === totalPaginas}
            >
              Próxima
            </button>
          </div>
        )}
      </div>
    </div>
  );
}