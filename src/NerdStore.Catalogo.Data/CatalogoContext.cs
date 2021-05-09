using Microsoft.EntityFrameworkCore;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.DomainObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Data
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        public CatalogoContext(DbContextOptions<CatalogoContext> options)
            : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            

            //Pega todas a entidades mapeadas e filtra as propriedades que são do tipo string
            var propriedadesTipoString = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string)));

            foreach (var propriedade in propriedadesTipoString)
            {
                propriedade.SetColumnType("varchar(100)");
            }

            //aplica no contexto todas as configuracoes definidas para as entidades
            //pega as entidades que implementam IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            //pega do ChangeTracker do EF todas a propridades com nome 'DataCadastro'
            var entidadesComPropridadeDataCadastro = ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null);

            foreach (var entidade in entidadesComPropridadeDataCadastro)
            {
                //seta a propridade DataCastro daquela entidade para agora
                if (entidade.State == EntityState.Added)
                {
                    entidade.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entidade.State == EntityState.Modified)
                {
                    entidade.Property("DataCadastro").IsModified = false;
                }
            }

            return await base.SaveChangesAsync() > 0;
        }
    }
}
