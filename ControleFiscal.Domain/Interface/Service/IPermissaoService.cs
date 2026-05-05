using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Infrastructure.Sql.Entity;

namespace ControleFiscal.Services.Interfaces
{
    public interface IPermissaoService
    {
        List<Permissao> ListarPermissoes();
        Permissao ObterPermissao(int id);
        Permissao CriarPermissao(string nome, string descricao);
        Permissao AlterarPermissao(int id, string nome, string descricao);
        void DeletarPermissao(int id);

        List<PermissaoUsuarioDTO> ListarPermissoesDoUsuario(int usuarioId);
        PermissaoUsuario VincularPermissao(int usuarioId, int permissaoId);
        void DesvincularPermissao(int permissaoUsuarioId);

        List<Usuario> ListarUsuarios();
        bool UsuarioTemPermissao(int usuarioId, string nomePermissao);
    }
}
