using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleFiscal.ApplicationCore.Service
{
    public class PermissaoService : IPermissaoService
    {
        private readonly ContextLocalContext _context;

        public PermissaoService(ContextLocalContext context)
        {
            _context = context;
        }

        public List<Permissao> ListarPermissoes()
        {
            return _context.Permissoes
                .OrderBy(p => p.Nome)
                .ToList();
        }

        public Permissao ObterPermissao(int id)
        {
            return _context.Permissoes.Find(id)
                ?? throw new KeyNotFoundException($"Permissão {id} não encontrada.");
        }

        public Permissao CriarPermissao(string nome, string descricao)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");

            var entity = new Permissao
            {
                Nome = nome.Trim().ToUpperInvariant(),
                Descricao = descricao?.Trim() ?? ""
            };

            _context.Permissoes.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Permissao AlterarPermissao(int id, string nome, string descricao)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");

            var entity = ObterPermissao(id);
            entity.Nome = nome.Trim().ToUpperInvariant();
            entity.Descricao = descricao?.Trim() ?? "";

            _context.Permissoes.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public void DeletarPermissao(int id)
        {
            var entity = ObterPermissao(id);
            _context.Permissoes.Remove(entity);
            _context.SaveChanges();
        }

        public List<PermissaoUsuarioDTO> ListarPermissoesDoUsuario(int usuarioId)
        {
            var permissoes = _context.Permissoes.ToList();
            var permisoesUsuario = _context.PermissoesUsuarios.Where(x => x.UsuarioId == usuarioId).ToList();

            foreach (var item in permisoesUsuario)
            {
                item.Permissao = permissoes.FirstOrDefault(x => x.Id == item.PermissaoId);
            }

            return permisoesUsuario.Select(x => new PermissaoUsuarioDTO
            {
                Id = x.Id,
                UsuarioId = x.UsuarioId,
                PermissaoId = x.PermissaoId,
                Permissao = x.Permissao != null ? new PermissaoDTO
                {
                    Id = x.Permissao.Id,
                    Nome = x.Permissao.Nome,
                    Descricao = x.Permissao.Descricao
                } : null
            }).ToList();
        }

        public PermissaoUsuario VincularPermissao(int usuarioId, int permissaoId)
        {
            var jaExiste = _context.PermissoesUsuarios
                .Any(pu => pu.UsuarioId == usuarioId && pu.PermissaoId == permissaoId);

            if (jaExiste)
                throw new InvalidOperationException("Usuário já possui esta permissão.");

            var entity = new PermissaoUsuario
            {
                UsuarioId = usuarioId,
                PermissaoId = permissaoId
            };

            _context.PermissoesUsuarios.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public void DesvincularPermissao(int permissaoUsuarioId)
        {
            var entity = _context.PermissoesUsuarios.Find(permissaoUsuarioId)
                ?? throw new KeyNotFoundException("Vínculo não encontrado.");

            _context.PermissoesUsuarios.Remove(entity);
            _context.SaveChanges();
        }

        public List<Usuario> ListarUsuarios()
        {
            return _context.Logins
                .OrderBy(u => u.Nome)
                .ToList();
        }

        public bool UsuarioTemPermissao(int usuarioId, string nomePermissao)
        {
            return _context.PermissoesUsuarios
                .Include(pu => pu.Permissao)
                .Any(pu => pu.UsuarioId == usuarioId
                        && pu.Permissao!.Nome == nomePermissao.ToUpperInvariant());
        }
    }
}
