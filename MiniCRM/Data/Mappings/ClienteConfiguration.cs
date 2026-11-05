using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCRM.Models;

namespace MiniCRM.Data.Mappings
{
    namespace MiniCrmAgencia.Data.Mappings
    {
        public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
        {
            public void Configure(EntityTypeBuilder<Cliente> builder)
            {
                // Nome da tabela
                builder.ToTable("Clientes");

                // Chave Primária
                builder.HasKey(c => c.Id);

                // Mapeamento de Propriedades
                builder.Property(c => c.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(c => c.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                builder.Property(c => c.Telefone)
                    .HasMaxLength(20);

                builder.Property(c => c.DataCriacao)
                    .IsRequired();

                // Relacionamento 1 : N (Um cliente tem muitas campanhas)
                builder.HasMany(c => c.Campanhas)
                    .WithOne(c => c.Cliente)
                    .HasForeignKey(c => c.ClienteId)
                    // Restrict evita que um cliente seja deletado se ele tiver campanhas atreladas
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
}
