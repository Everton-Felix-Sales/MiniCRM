using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCRM.Models;

namespace MiniCRM.Data.Mappings
{
    namespace MiniCrmAgencia.Data.Mappings
    {
        public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
        {
            public void Configure(EntityTypeBuilder<Usuario> builder)
            {
                builder.ToTable("Usuarios");

                builder.HasKey(u => u.Id);

                builder.Property(u => u.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                builder.Property(u => u.Senha)
                    .IsRequired()
                    .HasMaxLength(255); // Tamanho bom para armazenar hashes (ex: bcrypt ou SHA256)

                builder.Property(u => u.Perfil)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(u => u.Ativo)
                    .IsRequired();

                builder.Property(u => u.DataCriacao)
                    .IsRequired();
            }
        }
    }
}
