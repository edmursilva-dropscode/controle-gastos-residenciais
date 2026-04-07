import { api } from "./api";
import { Transacao, TransacaoCreateDto } from "../types/Transacao";

// Busca todas as transações cadastradas na API
export async function listarTransacoes(): Promise<Transacao[]> {
  const response = await api.get("/transacoes");
  return response.data;
}

// Envia uma nova transação para a API
export async function criarTransacao(data: TransacaoCreateDto): Promise<void> {
  await api.post("/transacoes", data);
}

// Exclui uma transação pelo identificador
export async function excluirTransacao(id: number): Promise<void> {
  await api.delete(`/transacoes/${id}`);
}