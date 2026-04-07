import { useEffect, useState } from "react";
import { api } from "../services/api";
import { listarCategorias } from "../services/categoriaService";
import { listarTransacoes, criarTransacao, excluirTransacao,} from "../services/transacaoService";
import { Pessoa } from "../types/Pessoa";
import { Categoria } from "../types/Categoria";
import { Transacao } from "../types/Transacao";
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

// Página responsável pelo cadastro, listagem e exclusão de transações
export function TransacoesPage() {
  // Lista de transações carregadas da API
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);

  // Lista de pessoas para o select
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);

  // Lista de categorias para o select
  const [categorias, setCategorias] = useState<Categoria[]>([]);

  // Campos do formulário
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState("");
  const [tipo, setTipo] = useState("");
  const [categoriaId, setCategoriaId] = useState("");
  const [pessoaId, setPessoaId] = useState("");

  // Controle de paginação
  const [paginaAtual, setPaginaAtual] = useState(1);
  const itensPorPagina = 10;

  // Carrega os dados ao abrir a tela
  useEffect(() => {
    carregarDados();
  }, []);

  // Mantém a página atual sincronizada com a quantidade de páginas existente.
  // Isso evita ficar preso em uma página inexistente após exclusões.
  useEffect(() => {
    const totalPaginasCalculado = Math.max(
      1,
      Math.ceil(transacoes.length / itensPorPagina)
    );

    if (paginaAtual > totalPaginasCalculado) {
      setPaginaAtual(totalPaginasCalculado);
    }
  }, [transacoes, paginaAtual]);

  // Busca transações, pessoas e categorias da API
  async function carregarDados() {
    try {
      const [transacoesData, pessoasResponse, categoriasData] =
        await Promise.all([
          listarTransacoes(),
          api.get("/pessoas"),
          listarCategorias(),
        ]);

      setTransacoes(transacoesData);
      setPessoas(pessoasResponse.data);
      setCategorias(categoriasData);
    } catch (error) {
      console.error("Erro ao carregar dados", error);
      alert("Não foi possível carregar os dados.");
    }
  }

  // Limpa os campos do formulário
  function limparFormulario() {
    setDescricao("");
    setValor("");
    setTipo("");
    setCategoriaId("");
    setPessoaId("");
  }

  // Extrai mensagens amigáveis vindas da API
  function obterMensagensDeValidacao(
    resposta: RespostaValidacao | string | undefined
  ): string | null {
    if (!resposta) {
      return null;
    }

    if (typeof resposta === "string") {
      return resposta;
    }

    // Junta todas as mensagens de validação
    if (resposta.errors) {
      return Object.values(resposta.errors).flat().join("\n");
    }

    // Suporte para message e mensagem
    return resposta.message || resposta.mensagem || null;
  }

  // Formata valor monetário no padrão brasileiro
  function formatarMoeda(valor: number): string {
    return valor.toLocaleString("pt-BR", {
      style: "currency",
      currency: "BRL",
    });
  }

  // Converte tipo numérico em texto amigável
  function obterTextoTipo(valor: number): string {
    return valor === 1 ? "Despesa" : "Receita";
  }

  // Filtra categorias conforme o tipo selecionado
  function obterCategoriasCompativeis(): Categoria[] {
    if (!tipo) {
      return categorias;
    }

    const tipoNum = Number(tipo);

    return categorias.filter((c) => {
      // Categoria aceita ambos
      if (c.finalidade === 3) {
        return true;
      }

      // Categoria deve bater com o tipo da transação
      return c.finalidade === tipoNum;
    });
  }

  // Salva uma nova transação mantendo a página atual da listagem
  async function salvarTransacao() {
    try {
      // Validação de descrição
      if (!descricao.trim()) {
        alert("A descrição é obrigatória.");
        return;
      }

      if (descricao.trim().length > 400) {
        alert("A descrição deve ter no máximo 400 caracteres.");
        return;
      }

      // Validação de valor
      if (!valor || Number(valor) <= 0) {
        alert("O valor deve ser maior que zero.");
        return;
      }

      // Validação dos campos obrigatórios
      if (!tipo || !categoriaId || !pessoaId) {
        alert("Preencha todos os campos.");
        return;
      }

      // Envia os dados para a API
      await criarTransacao({
        descricao: descricao.trim(),
        valor: Number(valor),
        tipo: Number(tipo),
        categoriaId: Number(categoriaId),
        pessoaId: Number(pessoaId),
      });

      limparFormulario();
      await carregarDados();
    } catch (error: unknown) {
      console.error("Erro ao salvar transação", error);

      const erro = error as ErroComResposta;

      // Tenta obter a mensagem retornada pelo back-end
      const msg = obterMensagensDeValidacao(erro.response?.data);

      if (msg) {
        alert(msg);
        return;
      }

      // Exibe mensagem genérica caso a API não retorne detalhes
      alert("Não foi possível salvar a transação.");
    }
  }

  // Exclui uma transação mantendo a página atual sempre que possível
  async function deletarTransacao(id: number) {
    const confirmar = window.confirm(
      "Deseja realmente excluir esta transação?"
    );

    if (!confirmar) {
      return;
    }

    try {
      await excluirTransacao(id);
      await carregarDados();
    } catch (error: unknown) {
      console.error("Erro ao excluir transação", error);

      const erro = error as ErroComResposta;

      // Tenta obter a mensagem retornada pelo back-end
      const msg = obterMensagensDeValidacao(erro.response?.data);

      if (msg) {
        alert(msg);
        return;
      }

      // Exibe mensagem genérica caso a API não retorne detalhes
      alert("Não foi possível excluir a transação.");
    }
  }

  // Total de páginas disponíveis.
  // Usa Math.max para garantir pelo menos 1 página lógica.
  const totalPaginas = Math.max(1, Math.ceil(transacoes.length / itensPorPagina));

  // Ajusta a página usada na renderização para nunca ultrapassar o total disponível.
  const paginaAtualAjustada = Math.min(paginaAtual, totalPaginas);

  // Calcula os índices da paginação com base na página ajustada
  const indiceInicial = (paginaAtualAjustada - 1) * itensPorPagina;
  const indiceFinal = indiceInicial + itensPorPagina;

  // Lista de transações exibidas na página atual
  const transacoesPaginadas = transacoes.slice(indiceInicial, indiceFinal);

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
        <h1 className="page-title">Cadastro de Transações</h1>

        {/* Linha 1 - campos principais */}
        <div className="form-row">
          <input
            className="input"
            placeholder="Descrição"
            value={descricao}
            onChange={(e) => setDescricao(e.target.value)}
          />

          <input
            className="input"
            type="number"
            placeholder="Valor"
            value={valor}
            onChange={(e) => setValor(e.target.value)}
          />
        </div>

        {/* Linha 2 - combos */}
        <div className="form-row">
          <select
            className="input"
            value={tipo}
            onChange={(e) => {
              setTipo(e.target.value);
              setCategoriaId("");
            }}
          >
            <option value="">Tipo</option>
            <option value="1">Despesa</option>
            <option value="2">Receita</option>
          </select>

          <select
            className="input"
            value={categoriaId}
            onChange={(e) => setCategoriaId(e.target.value)}
          >
            <option value="">Categoria</option>
            {obterCategoriasCompativeis().map((c) => (
              <option key={c.id} value={c.id}>
                {c.descricao}
              </option>
            ))}
          </select>

          <select
            className="input"
            value={pessoaId}
            onChange={(e) => setPessoaId(e.target.value)}
          >
            <option value="">Pessoa</option>
            {pessoas.map((p) => (
              <option key={p.id} value={p.id}>
                {p.nome}
              </option>
            ))}
          </select>
        </div>

        {/* Botão de ação */}
        <div style={{ marginTop: "10px", marginBottom: "16px" }}>
          <button className="button" onClick={salvarTransacao}>
            Cadastrar
          </button>
        </div>

        {/* Tabela de transações */}
        <div className="table-wrapper">
          <table className="data-table">
            <thead>
              <tr>
                <th>Descrição</th>
                <th>Tipo</th>
                <th>Valor</th>
                <th>Pessoa</th>
                <th>Categoria</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {transacoesPaginadas.length > 0 ? (
                transacoesPaginadas.map((t) => (
                  <tr key={t.id}>
                    <td>{t.descricao}</td>
                    <td>{obterTextoTipo(t.tipo)}</td>
                    <td>{formatarMoeda(t.valor)}</td>
                    <td>{t.pessoaNome}</td>
                    <td>{t.categoriaDescricao}</td>
                    <td>
                      <button
                        className="button button-danger"
                        onClick={() => deletarTransacao(t.id)}
                      >
                        Excluir
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={6} className="table-empty">
                    Nenhuma transação cadastrada.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>

        {/* Paginação */}
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