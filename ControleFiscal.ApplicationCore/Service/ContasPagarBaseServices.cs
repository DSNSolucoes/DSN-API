using ControleFiscal.Domain.DTO.ContasPagar;
using ControleFiscal.Domain.Model.ContasPagar;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleFiscal.ApplicationCore.Service
{
    public class FornecedorCPService : IFornecedorCPService
    {
        private readonly ContextLocalContext _context;
        public FornecedorCPService(ContextLocalContext context) => _context = context;

        public List<FornecedorDTO> Listar(int empresaId)
        {
            return _context.Fornecedores
                .Where(f => f.Id_Empresa == empresaId)
                .OrderBy(f => f.NmFornecedor)
                .Select(f => new FornecedorDTO
                {
                    CdFornecedor = f.CdFornecedor,
                    IdEmpresa = f.Id_Empresa,
                    Nome = f.NmFornecedor,
                    NomeFantasia = f.NomeFantasia,
                    Documento = f.Documento,
                    Email = f.Email,
                    Telefone = f.Telefone,
                    Status = f.Status ?? "ATIVO"
                }).ToList();
        }

        public FornecedorDTO Obter(int empresaId, int cdFornecedor)
        {
            var f = _context.Fornecedores
                .FirstOrDefault(x => x.CdFornecedor == cdFornecedor && x.Id_Empresa == empresaId)
                ?? throw new KeyNotFoundException($"Fornecedor {cdFornecedor} não encontrado.");
            return new FornecedorDTO
            {
                CdFornecedor = f.CdFornecedor, IdEmpresa = f.Id_Empresa,
                Nome = f.NmFornecedor, NomeFantasia = f.NomeFantasia,
                Documento = f.Documento, Email = f.Email,
                Telefone = f.Telefone, Status = f.Status ?? "ATIVO"
            };
        }

        public Fornecedor Criar(FornecedorSalvarModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nome))
                throw new ArgumentException("Nome é obrigatório.");

            var entity = new Fornecedor
            {
                Id_Empresa = model.IdEmpresa,
                NmFornecedor = model.Nome.Trim(),
                NomeFantasia = model.NomeFantasia?.Trim(),
                Documento = model.Documento?.Trim(),
                Email = model.Email?.Trim(),
                Telefone = model.Telefone?.Trim(),
                Observacoes = model.Observacoes,
                Status = model.Status ?? "ATIVO",
                DataCriacao = DateTime.Now
            };
            _context.Fornecedores.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Fornecedor Alterar(int cdFornecedor, FornecedorSalvarModel model)
        {
            var entity = _context.Fornecedores
                .FirstOrDefault(x => x.CdFornecedor == cdFornecedor && x.Id_Empresa == model.IdEmpresa)
                ?? throw new KeyNotFoundException($"Fornecedor {cdFornecedor} não encontrado.");

            entity.NmFornecedor = model.Nome.Trim();
            entity.NomeFantasia = model.NomeFantasia?.Trim();
            entity.Documento = model.Documento?.Trim();
            entity.Email = model.Email?.Trim();
            entity.Telefone = model.Telefone?.Trim();
            entity.Observacoes = model.Observacoes;
            entity.Status = model.Status ?? entity.Status;
            entity.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return entity;
        }

        public void Inativar(int cdFornecedor)
        {
            var entity = _context.Fornecedores
                .FirstOrDefault(x => x.CdFornecedor == cdFornecedor)
                ?? throw new KeyNotFoundException($"Fornecedor {cdFornecedor} não encontrado.");
            entity.Status = "INATIVO";
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }
    }

    public class CategoriaContaPagarService : ICategoriaContaPagarService
    {
        private readonly ContextLocalContext _context;
        public CategoriaContaPagarService(ContextLocalContext context) => _context = context;

        public List<CategoriaContaPagarDTO> Listar(int empresaId)
        {
            return _context.CategoriasContaPagar
                .Where(c => c.IdEmpresa == empresaId)
                .OrderBy(c => c.Nome)
                .Select(c => new CategoriaContaPagarDTO
                {
                    Id = c.Id,
                    IdEmpresa = c.IdEmpresa,
                    Nome = c.Nome,
                    IdCategoriaPai = c.IdCategoriaPai,
                    NomeCategoriaPai = c.CategoriaPai != null ? c.CategoriaPai.Nome : null,
                    Status = c.Status
                }).ToList();
        }

        public CategoriaContaPagarDTO Obter(int empresaId, int id)
        {
            var c = _context.CategoriasContaPagar
                .Include(x => x.CategoriaPai)
                .FirstOrDefault(x => x.Id == id && x.IdEmpresa == empresaId)
                ?? throw new KeyNotFoundException($"Categoria {id} não encontrada.");
            return new CategoriaContaPagarDTO
            {
                Id = c.Id, IdEmpresa = c.IdEmpresa, Nome = c.Nome,
                IdCategoriaPai = c.IdCategoriaPai,
                NomeCategoriaPai = c.CategoriaPai?.Nome,
                Status = c.Status
            };
        }

        public CategoriaContaPagar Criar(CategoriaContaPagarSalvarModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nome))
                throw new ArgumentException("Nome é obrigatório.");

            var entity = new CategoriaContaPagar
            {
                IdEmpresa = model.IdEmpresa,
                Nome = model.Nome.Trim(),
                IdCategoriaPai = model.IdCategoriaPai,
                Status = model.Status ?? "ATIVA",
                DataCriacao = DateTime.Now
            };
            _context.CategoriasContaPagar.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public CategoriaContaPagar Alterar(int id, CategoriaContaPagarSalvarModel model)
        {
            var entity = _context.CategoriasContaPagar
                .FirstOrDefault(x => x.Id == id && x.IdEmpresa == model.IdEmpresa)
                ?? throw new KeyNotFoundException($"Categoria {id} não encontrada.");

            entity.Nome = model.Nome.Trim();
            entity.IdCategoriaPai = model.IdCategoriaPai;
            entity.Status = model.Status ?? entity.Status;
            entity.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return entity;
        }

        public void Inativar(int id)
        {
            var entity = _context.CategoriasContaPagar.Find(id)
                ?? throw new KeyNotFoundException($"Categoria {id} não encontrada.");
            entity.Status = "INATIVA";
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }
    }

    public class CentroCustoCPService : ICentroCustoCPService
    {
        private readonly ContextLocalContext _context;
        public CentroCustoCPService(ContextLocalContext context) => _context = context;

        public List<CentroCustoCPDTO> Listar(int empresaId)
        {
            return _context.CentrosCustoCP
                .Where(c => c.IdEmpresa == empresaId)
                .OrderBy(c => c.Nome)
                .Select(c => new CentroCustoCPDTO
                {
                    Id = c.Id,
                    IdEmpresa = c.IdEmpresa,
                    Nome = c.Nome,
                    Codigo = c.Codigo,
                    Status = c.Status
                }).ToList();
        }

        public CentroCustoCP Criar(CentroCustoCPSalvarModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nome))
                throw new ArgumentException("Nome é obrigatório.");

            var entity = new CentroCustoCP
            {
                IdEmpresa = model.IdEmpresa,
                Nome = model.Nome.Trim(),
                Codigo = model.Codigo?.Trim(),
                Status = model.Status ?? "ATIVO",
                DataCriacao = DateTime.Now
            };
            _context.CentrosCustoCP.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public CentroCustoCP Alterar(int id, CentroCustoCPSalvarModel model)
        {
            var entity = _context.CentrosCustoCP
                .FirstOrDefault(x => x.Id == id && x.IdEmpresa == model.IdEmpresa)
                ?? throw new KeyNotFoundException($"Centro de custo {id} não encontrado.");

            entity.Nome = model.Nome.Trim();
            entity.Codigo = model.Codigo?.Trim();
            entity.Status = model.Status ?? entity.Status;
            entity.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return entity;
        }

        public void Inativar(int id)
        {
            var entity = _context.CentrosCustoCP.Find(id)
                ?? throw new KeyNotFoundException($"Centro de custo {id} não encontrado.");
            entity.Status = "INATIVO";
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }
    }
}
