// Configura a comunicação HTTP com a API do back-end - http://localhost:5173
import axios from "axios";

// Instância do axios configurada para sua API .NET      
export const api = axios.create({
  baseURL: "https://localhost:7001/api",
});