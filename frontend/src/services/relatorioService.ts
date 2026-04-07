import { api } from "./api";
import {
  RelatorioCategoriaItem,
  RelatorioCategoriasResponse,
  RelatorioPessoaItem,
  RelatorioPessoasResponse,
  ResumoGeral,
} from "../types/Relatorio";

// Converte qualquer valor recebido em número seguro
function converterNumero(valor: unknown): number {
  if (typeof valor === "number") {
    return valor;
  }

  if (typeof valor === "string") {
    const numero = Number(valor);
    return Number.isNaN(numero) ? 0 : numero;
  }

  return 0;
}

// Normaliza o resumo geral retornado pela API.
// Isso ajuda a evitar quebra caso o back-end use nomes ligeiramente diferentes.
function normalizarResumoGeral(dados: unknown): ResumoGeral {
  const resumo = (dados ?? {}) as Record<string, unknown>;

  return {
    totalReceitas: converterNumero(
      resumo.totalReceitas ?? resumo.receitas ?? resumo.totalReceita
    ),
    totalDespesas: converterNumero(
      resumo.totalDespesas ?? resumo.despesas ?? resumo.totalDespesa
    ),
    saldo: converterNumero(resumo.saldo ?? resumo.saldoLiquido),
  };
}

// Busca e normaliza o relatório por pessoa
export async function obterRelatorioPessoas(): Promise<RelatorioPessoasResponse> {
  const response = await api.get("/relatorios/pessoas");
  const dados = response.data;

  // Tenta localizar a lista em formatos diferentes para evitar quebra
  const listaOriginal =
    dados?.pessoas ??
    dados?.itens ??
    dados?.dados ??
    (Array.isArray(dados) ? dados : []);

  // Tenta localizar o resumo geral em formatos diferentes
  const resumoOriginal =
    dados?.resumoGeral ?? dados?.totalGeral ?? dados?.totaisGerais ?? {};

  const pessoas: RelatorioPessoaItem[] = Array.isArray(listaOriginal)
    ? listaOriginal.map((item: Record<string, unknown>) => ({
        pessoaId: converterNumero(item.pessoaId ?? item.id),
        nome: String(item.nome ?? item.pessoaNome ?? ""),
        totalReceitas: converterNumero(
          item.totalReceitas ?? item.receitas ?? item.totalReceita
        ),
        totalDespesas: converterNumero(
          item.totalDespesas ?? item.despesas ?? item.totalDespesa
        ),
        saldo: converterNumero(item.saldo ?? item.saldoLiquido),
      }))
    : [];

  return {
    pessoas,
    resumoGeral: normalizarResumoGeral(resumoOriginal),
  };
}

// Busca e normaliza o relatório por categoria
export async function obterRelatorioCategorias(): Promise<RelatorioCategoriasResponse> {
  const response = await api.get("/relatorios/categorias");
  const dados = response.data;

  // Tenta localizar a lista em formatos diferentes para evitar quebra
  const listaOriginal =
    dados?.categorias ??
    dados?.itens ??
    dados?.dados ??
    (Array.isArray(dados) ? dados : []);

  // Tenta localizar o resumo geral em formatos diferentes
  const resumoOriginal =
    dados?.resumoGeral ?? dados?.totalGeral ?? dados?.totaisGerais ?? {};

  const categorias: RelatorioCategoriaItem[] = Array.isArray(listaOriginal)
    ? listaOriginal.map((item: Record<string, unknown>) => ({
        categoriaId: converterNumero(item.categoriaId ?? item.id),
        descricao: String(item.descricao ?? item.categoriaDescricao ?? ""),
        totalReceitas: converterNumero(
          item.totalReceitas ?? item.receitas ?? item.totalReceita
        ),
        totalDespesas: converterNumero(
          item.totalDespesas ?? item.despesas ?? item.totalDespesa
        ),
        saldo: converterNumero(item.saldo ?? item.saldoLiquido),
      }))
    : [];

  return {
    categorias,
    resumoGeral: normalizarResumoGeral(resumoOriginal),
  };
}