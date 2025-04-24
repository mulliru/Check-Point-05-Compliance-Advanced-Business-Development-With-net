using Microsoft.EntityFrameworkCore;
using Sprint03.Domain.Entities;

namespace Sprint03.Infrastructure.Data
{
    /// <summary>
    /// Contexto do banco de dados da aplicação.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Inicializa uma nova instância do <see cref="AppDbContext"/>.
        /// </summary>
        /// <param name="options">Opções de configuração do DbContext.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Representa a tabela de usuários no banco de dados.
        /// </summary>
        public DbSet<NomeUsuario> NomeUsuarios { get; set; }

        /// <summary>
        /// Configura os modelos do banco de dados.
        /// </summary>
        /// <param name="modelBuilder">Construtor de modelos do Entity Framework.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NomeUsuario>().HasKey(u => u.Id);
        }
    }
}
