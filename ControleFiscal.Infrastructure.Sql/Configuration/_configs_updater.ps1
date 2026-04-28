# Script temporário para atualizar todas as configurações EF Core
# Execute: pwsh -File _configs_updater.ps1

$p = Split-Path $MyInvocation.MyCommand.Path

# ── CaixaMovimentacaoConfiguration ──────────────────────────────────────────
Set-Content "$p\CaixaMovimentacaoConfiguration .cs" -Encoding UTF8 -Value @"
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class CaixaMovimentacaoConfiguration : IEntityTypeConfiguration<CaixaMovimentacao>
    {
        public void Configure(EntityTypeBuilder<CaixaMovimentacao> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CAIXA_MOVIMENTACAO");
            entity.HasIndex(e => e.Id, "PK_CAIXA_MOVIMENTACAO");
            entity.HasIndex(e => e.CaixaId, "IX_CAIXA_MOVIMENTACAO_CAIXA");
            entity.HasIndex(e => e.TipoValorCaixaId, "IX_CAIXA_MOVIMENTACAO_TIPO");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.CaixaId).HasColumnName("CAIXA_ID").HasColumnType("CHAR(36)").HasMaxLength(36).IsRequired();
            entity.Property(e => e.TipoValorCaixaId).HasColumnName("TIPO_VALOR_CAIXA_ID").HasColumnType("CHAR(36)").HasMaxLength(36).IsRequired();
            entity.Property(e => e.Valor).HasColumnName("VALOR").HasColumnType("NUMERIC(15,2)").IsRequired();
            entity.Property(e => e.DataCadastro).HasColumnName("DATA_CADASTRO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.Ativo).HasColumnName("ATIVO").HasMaxLength(1).HasDefaultValueSql("'V'");
            entity.Property(e => e.DataCompetencia).HasColumnName("DATA_COMPETENCIA").HasColumnType("TIMESTAMP");
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(100);
            entity.Property(e => e.DataRealizacao).HasColumnName("DATA_REALIZACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.AnexoNome).HasColumnName("ANEXO_NOME").HasMaxLength(100);
            entity.Property(e => e.AnexoArquivo).HasColumnName("ANEXO_ARQUIVO").HasColumnType("BLOB SUB_TYPE TEXT");
            entity.Property(e => e.AnexoContentType).HasColumnName("ANEXO_CONTENT_TYPE").HasMaxLength(10);
            entity.Property(e => e.NomeFuncionario).HasColumnName("NOME_FUNCIONARIO").HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<CaixaMovimentacao> entity);
    }
}
"@

# ── TipoValorCaixaConfiguration ─────────────────────────────────────────────
Set-Content "$p\TipoValorCaixaConfiguration.cs" -Encoding UTF8 -Value @"
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class TipoValorCaixaConfiguration : IEntityTypeConfiguration<TipoValorCaixa>
    {
        public void Configure(EntityTypeBuilder<TipoValorCaixa> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("TIPO_VALOR_CAIXA");
            entity.HasIndex(e => e.Id, "PK_TIPO_VALOR_CAIXA");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<TipoValorCaixa> entity);
    }
}
"@

# ── LojaConfiguration ────────────────────────────────────────────────────────
Set-Content "$p\LojaConfiguration.cs" -Encoding UTF8 -Value @"
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class LojaConfiguration : IEntityTypeConfiguration<Lojas>
    {
        public void Configure(EntityTypeBuilder<Lojas> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("LOJA");
            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.Caminho).HasColumnName("CAMINHO");
            entity.Property(e => e.Porta).HasColumnName("PORTA");
            entity.Property(e => e.Nome).HasColumnName("NOME");
            entity.Property(e => e.Host).HasColumnName("HOST");
            entity.Property(e => e.CNPJ).HasColumnName("CNPJ");
            entity.Property(e => e.PercentualST).HasColumnName("PERCENTUAL_ST");
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Lojas> entity);
    }
}
"@

# ── NcmConfiguration ─────────────────────────────────────────────────────────
Set-Content "$p\NcmConfiguration.cs" -Encoding UTF8 -Value @"
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class NcmConfiguration : IEntityTypeConfiguration<Ncm>
    {
        public void Configure(EntityTypeBuilder<Ncm> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("NCM");
            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.NCM).HasColumnName("NCM");
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO");
            entity.Property(e => e.Padrao).HasColumnName("PADRAO");
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Ncm> entity);
    }
}
"@

# ── UsuarioConfiguration ─────────────────────────────────────────────────────
Set-Content "$p\UsuarioConfiguration.cs" -Encoding UTF8 -Value @"
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("USUARIOS");
            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(100);
            entity.Property(e => e.Senha).HasColumnName("SENHA").HasMaxLength(1000);
            entity.Property(e => e.Login).HasColumnName("LOGIN").HasMaxLength(20);
            entity.Property(e => e.Produto).HasColumnName("PRODUTO");
            entity.Property(e => e.Relatorio).HasColumnName("RELATORIO");
            entity.Property(e => e.Fiscal).HasColumnName("FISCAL");
            entity.Property(e => e.Financeiro).HasColumnName("FINANCEIRO");
            entity.Property(e => e.Bloqueado).HasColumnName("BLOQUEADO");
            entity.Property(e => e.Dados_Bloqueio).HasColumnName("DADOS_BLOQUEIO");
            entity.Property(e => e.EmpresaId).HasColumnName("EMPRESA_ID");
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Usuario> entity);
    }
}
"@

# ── PermissaoConfiguration ───────────────────────────────────────────────────
Set-Content "$p\PermissaoConfiguration.cs" -Encoding UTF8 -Value @"
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class PermissaoConfiguration : IEntityTypeConfiguration<Permissao>
    {
        public void Configure(EntityTypeBuilder<Permissao> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("PERMISSAO");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(1000);
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            entity.HasMany(e => e.PermissoesUsuarios)
                  .WithOne(pu => pu.Permissao)
                  .HasForeignKey(pu => pu.PermissaoId);

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Permissao> entity);
    }
}
"@

# ── PermissaoUsuarioConfiguration ────────────────────────────────────────────
Set-Content "$p\PermissaoUsuarioConfiguration.cs" -Encoding UTF8 -Value @"
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

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID").HasColumnType("CHAR(36)").HasMaxLength(36);
            entity.Property(e => e.PermissaoId).HasColumnName("PERMISSAO_ID").HasColumnType("CHAR(36)").HasMaxLength(36);
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

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
"@

# ── FornecedorConfiguration ──────────────────────────────────────────────────
Set-Content "$p\FornecedorConfiguration.cs" -Encoding UTF8 -Value @"
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("FORNECEDOR");
            entity.HasIndex(e => e.Id, "PK_FORNECEDOR");
            entity.HasIndex(e => e.LojaId, "IX_FORNECEDOR_LOJA");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.NmFornecedor).HasColumnName("NM_FORNECEDOR").HasMaxLength(200);
            entity.Property(e => e.LojaId).HasColumnName("LOJA_ID").HasColumnType("CHAR(36)").HasMaxLength(36).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Fornecedor> entity);
    }
}
"@

Write-Host "All EF configs updated"
