import { useState } from "react";
import { HomePage } from "./pages/HomePage";
import { PessoasPage } from "./pages/PessoasPage";
import { CategoriasPage } from "./pages/CategoriasPage";
import { TransacoesPage } from "./pages/TransacoesPage";
import { RelatorioPessoasPage } from "./pages/RelatorioPessoasPage";
import { RelatorioCategoriasPage } from "./pages/RelatorioCategoriasPage";
import "./App.css";

// Define as opções disponíveis de navegação interna da aplicação
type TelaAtiva =
  | "home"
  | "pessoas"
  | "categorias"
  | "transacoes"
  | "relatorio-pessoas"
  | "relatorio-categorias";

// Componente raiz da aplicação
function App() {
  // Controla qual tela está sendo exibida no momento.
  // A aplicação inicia na tela principal.
  const [telaAtiva, setTelaAtiva] = useState<TelaAtiva>("home");

  // Retorna uma classe extra para destacar visualmente o botão ativo do menu
  function obterClasseBotaoMenu(tela: TelaAtiva): string {
    return telaAtiva === tela ? "button menu-button-active" : "button";
  }

  // Renderiza a tela atual conforme a opção selecionada no menu
  function renderizarTela() {
    switch (telaAtiva) {
      case "home":
        return <HomePage />;

      case "pessoas":
        return <PessoasPage />;

      case "categorias":
        return <CategoriasPage />;

      case "transacoes":
        return <TransacoesPage />;

      case "relatorio-pessoas":
        return <RelatorioPessoasPage />;

      case "relatorio-categorias":
        return <RelatorioCategoriasPage />;

      default:
        return <HomePage />;
    }
  }

  return (
    <div>
      {/* Menu principal da aplicação */}
      <div className="top-menu">
        <button
          className={obterClasseBotaoMenu("home")}
          onClick={() => setTelaAtiva("home")}
        >
          Início
        </button>

        <button
          className={obterClasseBotaoMenu("pessoas")}
          onClick={() => setTelaAtiva("pessoas")}
        >
          Pessoas
        </button>

        <button
          className={obterClasseBotaoMenu("categorias")}
          onClick={() => setTelaAtiva("categorias")}
        >
          Categorias
        </button>

        <button
          className={obterClasseBotaoMenu("transacoes")}
          onClick={() => setTelaAtiva("transacoes")}
        >
          Transações
        </button>

        <button
          className={obterClasseBotaoMenu("relatorio-pessoas")}
          onClick={() => setTelaAtiva("relatorio-pessoas")}
        >
          Relatório por Pessoa
        </button>

        <button
          className={obterClasseBotaoMenu("relatorio-categorias")}
          onClick={() => setTelaAtiva("relatorio-categorias")}
        >
          Relatório por Categoria
        </button>
      </div>

      {renderizarTela()}
    </div>
  );
}

export default App;