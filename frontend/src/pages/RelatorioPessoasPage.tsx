import { useEffect, useState } from "react";
import { obterRelatorioPessoas } from "../services/relatorioService";
import { RelatorioPessoaItem, ResumoGeral } from "../types/Relatorio";
import "../App.css";

// Página responsável por exibir o relatório consolidado por pessoa
export function RelatorioPessoasPage() {
  // Lista retornada pela API com os totais por pessoa
  const [pessoas, setPessoas] = useState<RelatorioPessoaItem[]>([]);

  // Resumo geral consolidado de todas as pessoas
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

      const dados = await obterRelatorioPessoas();

      setPessoas(dados.pessoas);
      setResumoGeral(dados.resumoGeral);
    } catch (error) {
      console.error("Erro ao buscar relatório por pessoa", error);
      setErro("Não foi possível carregar o relatório por pessoa.");
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
          <h1 className="page-title">Relatório por Pessoa</h1>
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
          <h1 className="page-title">Relatório por Pessoa</h1>
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
        <h1 className="page-title">Relatório por Pessoa</h1>

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

        {/* Tabela com os totais por pessoa */}
        <div className="table-wrapper">
          <table className="data-table">
            <thead>
              <tr>
                <th>Pessoa</th>
                <th>Receitas</th>
                <th>Despesas</th>
                <th>Saldo</th>
              </tr>
            </thead>
            <tbody>
              {pessoas.length > 0 ? (
                pessoas.map((pessoa) => (
                  <tr key={pessoa.pessoaId}>
                    <td>{pessoa.nome}</td>
                    <td>{formatarMoeda(pessoa.totalReceitas)}</td>
                    <td>{formatarMoeda(pessoa.totalDespesas)}</td>
                    <td>{formatarMoeda(pessoa.saldo)}</td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={4} className="table-empty">
                    Nenhum dado encontrado para o relatório por pessoa.
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