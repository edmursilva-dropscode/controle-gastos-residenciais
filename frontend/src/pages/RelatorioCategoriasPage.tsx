import { useEffect, useState } from "react";
import { obterRelatorioCategorias } from "../services/relatorioService";
import { RelatorioCategoriaItem, ResumoGeral } from "../types/Relatorio";
import "../App.css";

// Página responsável por exibir o relatório consolidado por categoria
export function RelatorioCategoriasPage() {
  // Lista retornada pela API com os totais por categoria
  const [categorias, setCategorias] = useState<RelatorioCategoriaItem[]>([]);

  // Resumo geral consolidado de todas as categorias
  const [resumoGeral, setResumoGeral] = useState<ResumoGeral>({
    totalReceitas: 0,
    totalDespesas: 0,
    saldo: 0,
  });

  // Controle de carregamento da tela
  const [carregando, setCarregando] = useState(true);

  // Mensagem de erro para falha na consulta
  const [erro, setErro] = useState("");

  // Carrega o relatório ao abrir a tela
  useEffect(() => {
    carregarRelatorio();
  }, []);

  // Busca os dados do relatório no back-end
  async function carregarRelatorio() {
    try {
      setCarregando(true);
      setErro("");

      const dados = await obterRelatorioCategorias();

      setCategorias(dados.categorias);
      setResumoGeral(dados.resumoGeral);
    } catch (error) {
      console.error("Erro ao buscar relatório por categoria", error);
      setErro("Não foi possível carregar o relatório por categoria.");
    } finally {
      setCarregando(false);
    }
  }

  // Formata valores monetários no padrão brasileiro
  function formatarMoeda(valor: number): string {
    return valor.toLocaleString("pt-BR", {
      style: "currency",
      currency: "BRL",
    });
  }

  // Exibe estado de carregamento
  if (carregando) {
    return (
      <div className="page">
        <div className="card">
          <h1 className="page-title">Relatório por Categoria</h1>
          <p>Carregando relatório...</p>
        </div>
      </div>
    );
  }

  // Exibe estado de erro
  if (erro) {
    return (
      <div className="page">
        <div className="card">
          <h1 className="page-title">Relatório por Categoria</h1>
          <p>{erro}</p>

          <div style={{ marginTop: "16px" }}>
            <button className="button" onClick={carregarRelatorio}>
              Tentar novamente
            </button>
          </div>
        </div>
      </div>
    );
  }

  // Renderização principal da tela
  return (
    <div className="page">
      <div className="card">
        <h1 className="page-title">Relatório por Categoria</h1>

        {/* Bloco com o resumo geral consolidado */}
        <div
          style={{
            marginBottom: "20px",
            padding: "16px",
            border: "1px solid #ddd",
            borderRadius: "8px",
            backgroundColor: "#fafafa",
          }}
        >
          <h2 style={{ marginTop: 0, marginBottom: "12px" }}>Total Geral</h2>

          <div className="form-row">
            <div>
              <strong>Receitas:</strong> {formatarMoeda(resumoGeral.totalReceitas)}
            </div>

            <div>
              <strong>Despesas:</strong> {formatarMoeda(resumoGeral.totalDespesas)}
            </div>

            <div>
              <strong>Saldo:</strong> {formatarMoeda(resumoGeral.saldo)}
            </div>
          </div>
        </div>

        {/* Tabela com os totais por categoria */}
        <div className="table-wrapper">
          <table className="data-table">
            <thead>
              <tr>
                <th>Categoria</th>
                <th>Receitas</th>
                <th>Despesas</th>
                <th>Saldo</th>
              </tr>
            </thead>
            <tbody>
              {categorias.length > 0 ? (
                categorias.map((categoria) => (
                  <tr key={categoria.categoriaId}>
                    <td>{categoria.descricao}</td>
                    <td>{formatarMoeda(categoria.totalReceitas)}</td>
                    <td>{formatarMoeda(categoria.totalDespesas)}</td>
                    <td>{formatarMoeda(categoria.saldo)}</td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={4} className="table-empty">
                    Nenhum dado encontrado para o relatório por categoria.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}