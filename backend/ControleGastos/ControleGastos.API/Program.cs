using ControleGastos.API.Data;
using ControleGastos.API.Repositories;
using ControleGastos.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registra os controladores da API.
builder.Services.AddControllers();

// Registra as dependências de pessoa.
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<IPessoaService, PessoaService>();

// Registra as dependências de categoria.
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

// Registra as dependências de transação.
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();

// Registra as dependências de relatório.
builder.Services.AddScoped<IRelatorioService, RelatorioService>();

// Adiciona os serviços do Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do CORS para permitir que o front-end React
// em http://localhost:5173 consuma esta API durante o desenvolvimento.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Registro do contexto do banco de dados.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configuração do pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilita o CORS antes do mapeamento dos controllers.
// Isso é necessário para o front-end conseguir chamar a API sem bloqueio do navegador.
app.UseCors("Frontend");

app.UseAuthorization();

app.MapControllers();

app.Run();