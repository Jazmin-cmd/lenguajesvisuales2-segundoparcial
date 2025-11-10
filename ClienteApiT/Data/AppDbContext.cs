using ClienteApiT.Models;
using Microsoft.EntityFrameworkCore;

namespace ClienteApiT.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ArchivoCliente> ArchivosClientes { get; set; }
        public DbSet<LogApi> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.CI).IsUnique();

            modelBuilder.Entity<ArchivoCliente>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Archivos)
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
