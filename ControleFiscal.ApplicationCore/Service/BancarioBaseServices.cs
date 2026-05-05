using ControleFiscal.Domain.DTO.Bancario;
using ControleFiscal.Domain.Model.Bancario;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleFiscal.ApplicationCore.Service
{
    public class BancoService : IBancoService
    {
        private readonly ContextLocalContext _context;

        public BancoService(ContextLocalContext context) => _context = context;

        public List<BancoDTO> Listar()
        {
            return _context.Bancos
                .OrderBy(b => b.Nome)
                .Select(b => new BancoDTO
                {
                    Id = b.Id,
                    Codigo = b.Codigo,
                    Ispb = b.Ispb,
                    Nome = b.Nome,
                    NomeReduzido = b.NomeReduzido,
                    ParticipaCompe = b.ParticipaCompe,
                    Status = b.Status
                }).ToList();
        }

        public BancoDTO Obter(int id)
        {
            var b = _context.Bancos.Find(id)
                ?? throw new KeyNotFoundException($"Banco {id} não encontrado.");
            return new BancoDTO
            {
                Id = b.Id, Codigo = b.Codigo, Ispb = b.Ispb,
                Nome = b.Nome, NomeReduzido = b.NomeReduzido,
                ParticipaCompe = b.ParticipaCompe, Status = b.Status
            };
        }

        public Banco Criar(BancoSalvarModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nome))
                throw new ArgumentException("Nome é obrigatório.");

            var entity = new Banco
            {
                Codigo = model.Codigo?.Trim(),
                Ispb = model.Ispb?.Trim(),
                Nome = model.Nome.Trim().ToUpperInvariant(),
                NomeReduzido = model.NomeReduzido?.Trim(),
                ParticipaCompe = model.ParticipaCompe,
                Status = model.Status
            };
            _context.Bancos.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Banco Alterar(int id, BancoSalvarModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nome))
                throw new ArgumentException("Nome é obrigatório.");

            var entity = _context.Bancos.Find(id)
                ?? throw new KeyNotFoundException($"Banco {id} não encontrado.");

            entity.Codigo = model.Codigo?.Trim();
            entity.Ispb = model.Ispb?.Trim();
            entity.Nome = model.Nome.Trim().ToUpperInvariant();
            entity.NomeReduzido = model.NomeReduzido?.Trim();
            entity.ParticipaCompe = model.ParticipaCompe;
            entity.Status = model.Status;
            entity.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return entity;
        }

        public void Deletar(int id)
        {
            var entity = _context.Bancos.Find(id)
                ?? throw new KeyNotFoundException($"Banco {id} não encontrado.");

            var temConta = _context.ContasBancarias.Any(c => c.IdBanco == id);
            if (temConta)
                throw new InvalidOperationException("Banco possui contas vinculadas. Inative o banco em vez de excluir.");

            entity.Status = "INATIVO";
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }
    }

    public class ContaBancariaService : IContaBancariaService
    {
        private readonly ContextLocalContext _context;

        public ContaBancariaService(ContextLocalContext context) => _context = context;

        public List<ContaBancariaDTO> Listar(int empresaId)
        {
            return _context.ContasBancarias
                .Include(c => c.Banco)
                .Where(c => c.IdEmpresa == empresaId)
                .OrderBy(c => c.Nome)
                .Select(c => new ContaBancariaDTO
                {
                    Id = c.Id,
                    IdEmpresa = c.IdEmpresa,
                    IdBanco = c.IdBanco,
                    NomeBanco = c.Banco != null ? c.Banco.NomeReduzido ?? c.Banco.Nome : null,
                    CodigoBanco = c.Banco != null ? c.Banco.Codigo : null,
                    Agencia = c.Agencia,
                    DigitoAgencia = c.DigitoAgencia,
                    NumeroConta = c.NumeroConta,
                    DigitoConta = c.DigitoConta,
                    TipoConta = c.TipoConta,
                    Nome = c.Nome,
                    Moeda = c.Moeda,
                    SaldoInicial = c.SaldoInicial,
                    DataSaldoInicial = c.DataSaldoInicial,
                    Status = c.Status,
                    Observacoes = c.Observacoes
                }).ToList();
        }

        public ContaBancariaDTO Obter(int empresaId, int id)
        {
            var c = _context.ContasBancarias
                .Include(x => x.Banco)
                .FirstOrDefault(x => x.IdEmpresa == empresaId && x.Id == id)
                ?? throw new KeyNotFoundException($"Conta {id} não encontrada.");

            return new ContaBancariaDTO
            {
                Id = c.Id, IdEmpresa = c.IdEmpresa, IdBanco = c.IdBanco,
                NomeBanco = c.Banco?.NomeReduzido ?? c.Banco?.Nome,
                CodigoBanco = c.Banco?.Codigo,
                Agencia = c.Agencia, DigitoAgencia = c.DigitoAgencia,
                NumeroConta = c.NumeroConta, DigitoConta = c.DigitoConta,
                TipoConta = c.TipoConta, Nome = c.Nome, Moeda = c.Moeda,
                SaldoInicial = c.SaldoInicial, DataSaldoInicial = c.DataSaldoInicial,
                Status = c.Status, Observacoes = c.Observacoes
            };
        }

        public ContaBancaria Criar(ContaBancariaSalvarModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nome))
                throw new ArgumentException("Nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(model.TipoConta))
                throw new ArgumentException("Tipo de conta é obrigatório.");

            var entity = new ContaBancaria
            {
                IdEmpresa = model.IdEmpresa,
                IdBanco = model.IdBanco,
                Agencia = model.Agencia,
                DigitoAgencia = model.DigitoAgencia,
                NumeroConta = model.NumeroConta,
                DigitoConta = model.DigitoConta,
                TipoConta = model.TipoConta,
                Nome = model.Nome.Trim(),
                Moeda = model.Moeda,
                SaldoInicial = model.SaldoInicial,
                DataSaldoInicial = model.DataSaldoInicial,
                Status = "ATIVA",
                Observacoes = model.Observacoes
            };
            _context.ContasBancarias.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public ContaBancaria Alterar(int id, ContaBancariaSalvarModel model)
        {
            var entity = _context.ContasBancarias.Find(id)
                ?? throw new KeyNotFoundException($"Conta {id} não encontrada.");

            if (entity.Status == "ENCERRADA")
                throw new InvalidOperationException("Conta encerrada não pode ser alterada.");

            entity.IdBanco = model.IdBanco;
            entity.Agencia = model.Agencia;
            entity.DigitoAgencia = model.DigitoAgencia;
            entity.NumeroConta = model.NumeroConta;
            entity.DigitoConta = model.DigitoConta;
            entity.TipoConta = model.TipoConta;
            entity.Nome = model.Nome.Trim();
            entity.Moeda = model.Moeda;
            entity.Observacoes = model.Observacoes;
            entity.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return entity;
        }

        public void Inativar(int id)
        {
            var entity = _context.ContasBancarias.Find(id)
                ?? throw new KeyNotFoundException($"Conta {id} não encontrada.");

            entity.Status = "INATIVA";
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }

        public SaldoContaDTO ObterSaldo(int empresaId, int id)
        {
            var conta = _context.ContasBancarias
                .FirstOrDefault(c => c.IdEmpresa == empresaId && c.Id == id)
                ?? throw new KeyNotFoundException($"Conta {id} não encontrada.");

            var lancamentos = _context.LancamentosBancarios
                .Where(l => l.IdContaBancaria == id && l.Status != "CANCELADO")
                .ToList();

            var totalCreditos = lancamentos.Where(l => l.Tipo == "CREDITO").Sum(l => l.Valor);
            var totalDebitos = lancamentos.Where(l => l.Tipo == "DEBITO").Sum(l => l.Valor);

            return new SaldoContaDTO
            {
                IdContaBancaria = id,
                NomeConta = conta.Nome,
                SaldoInicial = conta.SaldoInicial,
                TotalCreditos = totalCreditos,
                TotalDebitos = totalDebitos,
                SaldoAtual = conta.SaldoInicial + totalCreditos - totalDebitos
            };
        }

        public SaldoEmpresaDTO ObterSaldoEmpresa(int empresaId)
        {
            var contas = _context.ContasBancarias
                .Where(c => c.IdEmpresa == empresaId && c.Status == "ATIVA")
                .ToList();

            var contaIds = contas.Select(c => c.Id).ToList();
            var lancamentos = _context.LancamentosBancarios
                .Where(l => l.IdEmpresa == empresaId && contaIds.Contains(l.IdContaBancaria) && l.Status != "CANCELADO")
                .ToList();

            var saldoContas = contas.Select(c =>
            {
                var lancsContas = lancamentos.Where(l => l.IdContaBancaria == c.Id).ToList();
                var cr = lancsContas.Where(l => l.Tipo == "CREDITO").Sum(l => l.Valor);
                var db = lancsContas.Where(l => l.Tipo == "DEBITO").Sum(l => l.Valor);
                return new SaldoContaDTO
                {
                    IdContaBancaria = c.Id,
                    NomeConta = c.Nome,
                    SaldoInicial = c.SaldoInicial,
                    TotalCreditos = cr,
                    TotalDebitos = db,
                    SaldoAtual = c.SaldoInicial + cr - db
                };
            }).ToList();

            return new SaldoEmpresaDTO
            {
                IdEmpresa = empresaId,
                SaldoTotal = saldoContas.Sum(s => s.SaldoAtual),
                Contas = saldoContas
            };
        }
    }

    public class CategoriaFinanceiraService : ICategoriaFinanceiraService
    {
        private readonly ContextLocalContext _context;

        public CategoriaFinanceiraService(ContextLocalContext context) => _context = context;

        public List<CategoriaFinanceiraDTO> Listar(int empresaId)
        {
            return _context.CategoriasFinanceiras
                .Where(c => c.IdEmpresa == empresaId)
                .OrderBy(c => c.Nome)
                .Select(c => new CategoriaFinanceiraDTO
                {
                    Id = c.Id, IdEmpresa = c.IdEmpresa, Nome = c.Nome,
                    Tipo = c.Tipo, IdCategoriaPai = c.IdCategoriaPai, Status = c.Status
                }).ToList();
        }

        public CategoriaFinanceiraDTO Obter(int empresaId, int id)
        {
            var c = _context.CategoriasFinanceiras
                .FirstOrDefault(x => x.IdEmpresa == empresaId && x.Id == id)
                ?? throw new KeyNotFoundException($"Categoria {id} não encontrada.");

            return new CategoriaFinanceiraDTO
            {
                Id = c.Id, IdEmpresa = c.IdEmpresa, Nome = c.Nome,
                Tipo = c.Tipo, IdCategoriaPai = c.IdCategoriaPai, Status = c.Status
            };
        }

        public CategoriaFinanceira Criar(CategoriaFinanceiraSalvarModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nome))
                throw new ArgumentException("Nome é obrigatório.");

            var entity = new CategoriaFinanceira
            {
                IdEmpresa = model.IdEmpresa,
                Nome = model.Nome.Trim(),
                Tipo = model.Tipo,
                IdCategoriaPai = model.IdCategoriaPai,
                Status = "ATIVA"
            };
            _context.CategoriasFinanceiras.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public CategoriaFinanceira Alterar(int id, CategoriaFinanceiraSalvarModel model)
        {
            var entity = _context.CategoriasFinanceiras.Find(id)
                ?? throw new KeyNotFoundException($"Categoria {id} não encontrada.");

            entity.Nome = model.Nome.Trim();
            entity.Tipo = model.Tipo;
            entity.IdCategoriaPai = model.IdCategoriaPai;
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
            return entity;
        }

        public void Inativar(int id)
        {
            var entity = _context.CategoriasFinanceiras.Find(id)
                ?? throw new KeyNotFoundException($"Categoria {id} não encontrada.");

            entity.Status = "INATIVA";
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }
    }
}
