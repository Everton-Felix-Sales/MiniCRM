using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCRM.Models;

namespace MiniCRM.Data.Mappings
{
    namespace MiniCrmAgencia.Data.Mappings
    {
        public class CampanhaConfiguration : IEntityTypeConfiguration<Campanha>
        {
            public void Configure(EntityTypeBuilder<Campanha> builder)
            {
                builder.ToTable("Campanhas");

                builder.HasKey(c => c.Id);

                builder.Property(c => c.Nome)
                    .IsRequired()
                    .HasMaxLength(150);

                builder.Property(c => c.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(c => c.DataCriacao)
                    .IsRequired();

                // O relacionamento já foi configurado no Cliente, mas você 
                // também poderia configurá-lo aqui de forma inversa se preferir.
            }
        }
    }
}
