// Representa os dados de uma transação retornados pela API
export interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: number;
  categoriaId: number;
  categoriaDescricao?: string;
  pessoaId: number;
  pessoaNome?: string;
}

// Representa os dados enviados no cadastro de transação
export interface TransacaoCreateDto {
  descricao: string;
  valor: number;
  tipo: number;
  categoriaId: number;
  pessoaId: number;
}