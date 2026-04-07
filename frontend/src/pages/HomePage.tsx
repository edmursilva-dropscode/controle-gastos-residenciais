import logo from "../assets/logo.png";
import "../App.css";

// Página inicial da aplicação.
// Essa tela é exibida ao abrir o sistema antes de acessar os módulos do menu.
export function HomePage() {
  return (
    <div className="page">
      <div className="card home-card">
        {/* Área principal de apresentação do sistema */}
        <div className="home-content">
          {/* Ícone visual do sistema */}
          <div className="home-logo-wrapper">
            <img
              src={logo}
              alt="Logo do sistema"
              className="home-logo"
            />
          </div>

          {/* Título principal da tela inicial */}
          <h1 className="page-title home-title">
            Teste técnico - Desenvolvedor Full Stack
          </h1>

          {/* Texto de apoio da tela inicial */}
          <p className="home-subtitle">
            Sistema de controle de gastos residenciais com back-end em C#/.NET e
            front-end em React com TypeScript.
          </p>

          {/* Observação curta para orientar o uso */}
          <p className="home-text">
            Utilize o menu superior para acessar os cadastros, transações e
            relatórios do sistema.
          </p>
        </div>
      </div>
    </div>
  );
}