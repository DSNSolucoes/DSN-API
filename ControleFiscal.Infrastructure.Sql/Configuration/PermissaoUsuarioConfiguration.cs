
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class PermissaoUsuarioConfiguration : IEntityTypeConfiguration<PermissaoUsuario>
    {
        public void Configure(EntityTypeBuilder<PermissaoUsuario> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("PERMISSAO_USUARIO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID");
            entity.Property(e => e.PermissaoId).HasColumnName("PERMISSAO_ID");

            entity.HasOne(e => e.Usuario)
                  .WithMany(u => u.PermissoesUsuarios)
                  .HasForeignKey(e => e.UsuarioId);

            entity.HasOne(e => e.Permissao)
                  .WithMany(p => p.PermissoesUsuarios)
                  .HasForeignKey(e => e.PermissaoId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PermissaoUsuario> entity);
    }
}
