import { api } from "./api";
import { Categoria, CategoriaCreateDto } from "../types/Categoria";

// Busca todas as categorias cadastradas na API
export async function listarCategorias(): Promise<Categoria[]> {
  const response = await api.get("/categorias");
  return response.data;
}

// Envia uma nova categoria para a API
export async function criarCategoria(data: CategoriaCreateDto): Promise<void> {
  await api.post("/categorias", data);
}

// Exclui uma categoria pelo id
export async function excluirCategoria(id: number): Promise<void> {
  await api.delete(`/categorias/${id}`);
}