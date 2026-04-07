// Representa os dados de uma categoria retornados pela API
export interface Categoria {
  id: number;
  descricao: string;
  finalidade: number;
}

// Representa os dados enviados no cadastro de categoria
export interface CategoriaCreateDto {
  descricao: string;
  finalidade: number;
}