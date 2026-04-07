import { useEffect, useState } from "react";
import {
  criarCategoria,
  excluirCategoria,
  listarCategorias,
} from "../services/categoriaService";
import { Categoria } from "../types/Categoria";
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

// Página responsável pelo cadastro, listagem, exclusão e paginação de categorias
export function CategoriasPage() {
  // Lista de categorias carregadas da API
  const [categorias, setCategorias] = useState<Categoria[]>([]);

  // Descrição informada no formulário
  const [descricao, setDescricao] = useState("");

  // Finalidade selecionada no formulário
  const [finalidade, setFinalidade] = useState("");

  // Página atual da listagem
  const [paginaAtual, setPaginaAtual] = useState(1);

  // Quantidade de itens por página
  const itensPorPagina = 10;

  // Carrega as categorias ao abrir a tela
  useEffect(() => {
    carregarCategorias();
  }, []);

  // Mantém a página atual sincronizada com a quantidade de páginas existente.
  // Isso evita ficar preso em uma página inexistente após exclusões.
  useEffect(() => {
    const totalPaginasCalculado = Math.max(
      1,
      Math.ceil(categorias.length / itensPorPagina)
    );

    if (paginaAtual > totalPaginasCalculado) {
      setPaginaAtual(totalPaginasCalculado);
    }
  }, [categorias, paginaAtual]);

  // Busca todas as categorias cadastradas no back-end
  async function carregarCategorias() {
    try {
      const dados = await listarCategorias();
      setCategorias(dados);
    } catch (error) {
      console.error("Erro ao buscar categorias", error);
      alert("Não foi possível carregar a lista de categorias.");
    }
  }

  // Limpa os campos do formulário
  function limparFormulario() {
    setDescricao("");
    setFinalidade("");
  }

  // Converte o enum numérico em texto amigável para exibição
  function obterTextoFinalidade(valor: number): string {
    switch (valor) {
      case 1:
        return "Despesa";
      case 2:
        return "Receita";
      case 3:
        return "Ambas";
      default:
        return "Desconhecida";
    }
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

  // Salva uma nova categoria mantendo a página atual da listagem
  async function salvarCategoria() {
    try {
      // Validação da descrição
      if (!descricao.trim()) {
        alert("A descrição da categoria é obrigatória.");
        return;
      }

      if (descricao.trim().length > 400) {
        alert("A descrição da categoria deve ter no máximo 400 caracteres.");
        return;
      }

      // Validação da finalidade
      if (!finalidade) {
        alert("A finalidade da categoria é obrigatória.");
        return;
      }

      // Dados enviados para a API
      const payload = {
        descricao: descricao.trim(),
        finalidade: Number(finalidade),
      };

      await criarCategoria(payload);

      limparFormulario();
      await carregarCategorias();
    } catch (error: unknown) {
      console.error("Erro ao salvar categoria", error);

      const erroComResposta = error as ErroComResposta;

      // Tenta obter mensagens amigáveis da API
      const mensagensValidacao = obterMensagensDeValidacao(
        erroComResposta.response?.data
      );

      if (mensagensValidacao) {
        alert(mensagensValidacao);
        return;
      }

      alert("Não foi possível salvar os dados da categoria.");
    }
  }

  // Exclui uma categoria mantendo a página atual sempre que possível
  async function removerCategoria(id: number, descricaoCategoria: string) {
    const confirmar = window.confirm(
      `Deseja realmente excluir a categoria "${descricaoCategoria}"?`
    );

    if (!confirmar) {
      return;
    }

    try {
      await excluirCategoria(id);
      await carregarCategorias();
    } catch (error: unknown) {
      console.error("Erro ao excluir categoria", error);

      const erroComResposta = error as ErroComResposta;

      // Tenta obter mensagens amigáveis da API
      const mensagensValidacao = obterMensagensDeValidacao(
        erroComResposta.response?.data
      );

      if (mensagensValidacao) {
        alert(mensagensValidacao);
        return;
      }

      alert("Não foi possível excluir a categoria.");
    }
  }

  // Total de páginas disponíveis.
  // Usa Math.max para garantir pelo menos 1 página lógica.
  const totalPaginas = Math.max(1, Math.ceil(categorias.length / itensPorPagina));

  // Ajusta a página usada na renderização para nunca ultrapassar o total disponível.
  const paginaAtualAjustada = Math.min(paginaAtual, totalPaginas);

  // Calcula os índices da paginação com base na página ajustada
  const indiceInicial = (paginaAtualAjustada - 1) * itensPorPagina;
  const indiceFinal = indiceInicial + itensPorPagina;

  // Lista de categorias exibidas na página atual
  const categoriasPaginadas = categorias.slice(indiceInicial, indiceFinal);

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
        <h1 className="page-title">Cadastro de Categorias</h1>

        <div className="form-row">
          <input
            className="input"
            type="text"
            placeholder="Descrição"
            value={descricao}
            onChange={(e) => setDescricao(e.target.value)}
          />

          <select
            className="input"
            value={finalidade}
            onChange={(e) => setFinalidade(e.target.value)}
          >
            <option value="">Selecione a finalidade</option>
            <option value="1">Despesa</option>
            <option value="2">Receita</option>
            <option value="3">Ambas</option>
          </select>

          <button className="button" onClick={salvarCategoria}>
            Cadastrar
          </button>
        </div>

        <ul className="people-list">
          {categoriasPaginadas.map((categoria) => (
            <li key={categoria.id} className="person-item">
              <span>
                {categoria.descricao} -{" "}
                {obterTextoFinalidade(categoria.finalidade)}
              </span>

              <div className="actions">
                <button
                  className="button button-danger"
                  onClick={() =>
                    removerCategoria(categoria.id, categoria.descricao)
                  }
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