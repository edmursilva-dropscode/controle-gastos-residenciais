using ControleGastos.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.API.Data
{
    // Contexto principal do banco de dados
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; } // Tabela de pessoas.
        public DbSet<Categoria> Categorias { get; set; } // Tabela de categorias.
        public DbSet<Transacao> Transacoes { get; set; } // Tabela de transações.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Pessoa)
                .WithMany(p => p.Transacoes)
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Cascade); // Remove as transações ao excluir a pessoa.

            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Categoria)
                .WithMany(c => c.Transacoes)
                .HasForeignKey(t => t.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict); // Impede excluir categoria vinculada em transações.
        }
    }
}