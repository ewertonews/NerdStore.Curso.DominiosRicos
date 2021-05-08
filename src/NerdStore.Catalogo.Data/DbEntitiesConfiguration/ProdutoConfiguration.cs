using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Catalogo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Data.Mappings
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Imagem)
                .IsRequired()
                .HasColumnType("varchar(250)");

            //Transformando as propriedades o objeto de valor Dimensoes em colunas na tabela produto
            builder.OwnsOne(p => p.Dimensoes, pm =>
            {
                pm.Property(d => d.Altura)
                .HasColumnName("Altura")
                .HasColumnType("int");

                pm.Property(d => d.Largura)
                .HasColumnName("Largura")
                .HasColumnType("int");

                pm.Property(d => d.Profundidade)
                .HasColumnName("Profundidade")
                .HasColumnType("int");
            });

            builder.ToTable("Produtos");
        }
    }
}
