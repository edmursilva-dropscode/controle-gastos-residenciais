// Representa o resumo geral consolidado exibido no final dos relatórios
export type ResumoGeral = {
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
};

// Representa uma linha do relatório por pessoa
export type RelatorioPessoaItem = {
  pessoaId: number;
  nome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
};

// Estrutura final usada no front-end para o relatório por pessoa
export type RelatorioPessoasResponse = {
  pessoas: RelatorioPessoaItem[];
  resumoGeral: ResumoGeral;
};

// Representa uma linha do relatório por categoria
export type RelatorioCategoriaItem = {
  categoriaId: number;
  descricao: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
};

// Estrutura final usada no front-end para o relatório por categoria
export type RelatorioCategoriasResponse = {
  categorias: RelatorioCategoriaItem[];
  resumoGeral: ResumoGeral;
};