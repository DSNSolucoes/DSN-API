using ControleFiscal.Domain.DTO.Bancario;
using ControleFiscal.Domain.Model.Bancario;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ControleFiscal.ApplicationCore.Service
{
    public class LancamentoBancarioService : ILancamentoBancarioService
    {
        private readonly ContextLocalContext _context;

        public LancamentoBancarioService(ContextLocalContext context) => _context = context;

        public List<LancamentoBancarioDTO> Listar(int empresaId, int contaId, DateTime dataInicio, DateTime dataFim)
        {
            return _context.LancamentosBancarios
                .Include(l => l.Categoria)
                .Include(l => l.ContaBancaria)
                .Where(l => l.IdEmpresa == empresaId
                         && l.IdContaBancaria == contaId
                         && l.DataMovimentacao >= dataInicio
                         && l.DataMovimentacao <= dataFim)
                .OrderBy(l => l.DataMovimentacao)
                .ThenBy(l => l.Id)
                .Select(l => new LancamentoBancarioDTO
                {
                    Id = l.Id,
                    IdEmpresa = l.IdEmpresa,
                    IdContaBancaria = l.IdContaBancaria,
                    NomeConta = l.ContaBancaria != null ? l.ContaBancaria.Nome : null,
                    IdCategoria = l.IdCategoria,
                    NomeCategoria = l.Categoria != null ? l.Categoria.Nome : null,
                    DataMovimentacao = l.DataMovimentacao,
                    DataCompensacao = l.DataCompensacao,
                    Tipo = l.Tipo,
                    Valor = l.Valor,
                    DescricaoOriginal = l.DescricaoOriginal,
                    DescricaoNormalizada = l.DescricaoNormalizada,
                    Documento = l.Documento,
                    Origem = l.Origem,
                    Status = l.Status,
                    Fitid = l.Fitid,
                    Observacoes = l.Observacoes,
                    DataCriacao = l.DataCriacao
                }).ToList();
        }

        public LancamentoBancarioDTO Obter(int empresaId, int id)
        {
            var l = _context.LancamentosBancarios
                .Include(x => x.Categoria)
                .Include(x => x.ContaBancaria)
                .FirstOrDefault(x => x.IdEmpresa == empresaId && x.Id == id)
                ?? throw new KeyNotFoundException($"Lançamento {id} não encontrado.");

            return new LancamentoBancarioDTO
            {
                Id = l.Id, IdEmpresa = l.IdEmpresa, IdContaBancaria = l.IdContaBancaria,
                NomeConta = l.ContaBancaria?.Nome,
                IdCategoria = l.IdCategoria, NomeCategoria = l.Categoria?.Nome,
                DataMovimentacao = l.DataMovimentacao, DataCompensacao = l.DataCompensacao,
                Tipo = l.Tipo, Valor = l.Valor,
                DescricaoOriginal = l.DescricaoOriginal, DescricaoNormalizada = l.DescricaoNormalizada,
                Documento = l.Documento, Origem = l.Origem, Status = l.Status,
                Fitid = l.Fitid, Observacoes = l.Observacoes, DataCriacao = l.DataCriacao
            };
        }

        public LancamentoBancario Criar(LancamentoBancarioSalvarModel model)
        {
            if (model.Valor <= 0)
                throw new ArgumentException("Valor deve ser maior que zero.");
            if (string.IsNullOrWhiteSpace(model.Tipo) || (model.Tipo != "CREDITO" && model.Tipo != "DEBITO"))
                throw new ArgumentException("Tipo inválido. Use CREDITO ou DEBITO.");

            var conta = _context.ContasBancarias.Find(model.IdContaBancaria)
                ?? throw new KeyNotFoundException("Conta bancária não encontrada.");
            if (conta.Status != "ATIVA")
                throw new InvalidOperationException("Conta bancária não está ativa.");

            // Verificar se o mês está fechado
            var fechamento = _context.FechamentosBancarios.FirstOrDefault(f =>
                f.IdContaBancaria == model.IdContaBancaria &&
                f.Ano == model.DataMovimentacao.Year &&
                f.Mes == model.DataMovimentacao.Month &&
                f.Status == "FECHADO");

            if (fechamento != null)
                throw new InvalidOperationException("Mês fechado. Não é possível incluir lançamentos.");

            var descNormalizada = NormalizarDescricao(model.DescricaoOriginal);
            var hash = GerarHash(model.IdEmpresa, model.IdContaBancaria, model.DataMovimentacao,
                                 model.Tipo, model.Valor, descNormalizada, model.Documento, null);

            var entity = new LancamentoBancario
            {
                IdEmpresa = model.IdEmpresa,
                IdContaBancaria = model.IdContaBancaria,
                IdCategoria = model.IdCategoria,
                DataMovimentacao = model.DataMovimentacao,
                DataCompensacao = model.DataCompensacao,
                Tipo = model.Tipo,
                Valor = model.Valor,
                DescricaoOriginal = model.DescricaoOriginal,
                DescricaoNormalizada = descNormalizada,
                Documento = model.Documento,
                Origem = "MANUAL",
                Status = "PENDENTE",
                HashImportacao = hash,
                Observacoes = model.Observacoes,
                IdResponsavelCriacao = model.IdResponsavel
            };
            _context.LancamentosBancarios.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public LancamentoBancario Alterar(int id, LancamentoBancarioSalvarModel model)
        {
            var entity = _context.LancamentosBancarios.Find(id)
                ?? throw new KeyNotFoundException($"Lançamento {id} não encontrado.");

            if (entity.Status == "CANCELADO")
                throw new InvalidOperationException("Lançamento cancelado não pode ser alterado.");

            // Verificar mês fechado
            var fechamento = _context.FechamentosBancarios.FirstOrDefault(f =>
                f.IdContaBancaria == entity.IdContaBancaria &&
                f.Ano == entity.DataMovimentacao.Year &&
                f.Mes == entity.DataMovimentacao.Month &&
                f.Status == "FECHADO");

            if (fechamento != null)
                throw new InvalidOperationException("Mês fechado. Não é possível alterar lançamentos.");

            entity.IdCategoria = model.IdCategoria;
            entity.DataMovimentacao = model.DataMovimentacao;
            entity.DataCompensacao = model.DataCompensacao;
            entity.Tipo = model.Tipo;
            entity.Valor = model.Valor;
            entity.DescricaoOriginal = model.DescricaoOriginal;
            entity.DescricaoNormalizada = NormalizarDescricao(model.DescricaoOriginal);
            entity.Documento = model.Documento;
            entity.Observacoes = model.Observacoes;
            entity.IdResponsavelAtualizacao = model.IdResponsavel;
            entity.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return entity;
        }

        public void Cancelar(int id, int idResponsavel)
        {
            var entity = _context.LancamentosBancarios.Find(id)
                ?? throw new KeyNotFoundException($"Lançamento {id} não encontrado.");

            if (entity.Status == "CANCELADO")
                throw new InvalidOperationException("Lançamento já está cancelado.");

            var fechamento = _context.FechamentosBancarios.FirstOrDefault(f =>
                f.IdContaBancaria == entity.IdContaBancaria &&
                f.Ano == entity.DataMovimentacao.Year &&
                f.Mes == entity.DataMovimentacao.Month &&
                f.Status == "FECHADO");

            if (fechamento != null)
                throw new InvalidOperationException("Mês fechado. Não é possível cancelar lançamentos.");

            entity.Status = "CANCELADO";
            entity.IdResponsavelAtualizacao = idResponsavel;
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }

        private static string NormalizarDescricao(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao)) return string.Empty;
            return descricao.Trim().ToUpperInvariant()
                .Replace("  ", " ");
        }

        public static string GerarHash(int idEmpresa, int idConta, DateTime data,
            string tipo, decimal valor, string descricao, string? documento, string? fitid)
        {
            var raw = $"{idEmpresa}|{idConta}|{data:yyyy-MM-dd}|{tipo}|{valor:F2}|{descricao}|{documento}|{fitid}";
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }

    public class ConciliacaoBancariaService : IConciliacaoBancariaService
    {
        private readonly ContextLocalContext _context;

        public ConciliacaoBancariaService(ContextLocalContext context) => _context = context;

        public List<LancamentoBancarioDTO> ListarPendentes(int empresaId, int? contaId)
        {
            var query = _context.LancamentosBancarios
                .Include(l => l.ContaBancaria)
                .Where(l => l.IdEmpresa == empresaId && l.Status == "PENDENTE");

            if (contaId.HasValue)
                query = query.Where(l => l.IdContaBancaria == contaId.Value);

            return query.OrderBy(l => l.DataMovimentacao)
                .Select(l => new LancamentoBancarioDTO
                {
                    Id = l.Id, IdEmpresa = l.IdEmpresa, IdContaBancaria = l.IdContaBancaria,
                    NomeConta = l.ContaBancaria != null ? l.ContaBancaria.Nome : null,
                    DataMovimentacao = l.DataMovimentacao, Tipo = l.Tipo, Valor = l.Valor,
                    DescricaoNormalizada = l.DescricaoNormalizada, Documento = l.Documento,
                    Status = l.Status, Origem = l.Origem, DescricaoOriginal = l.DescricaoOriginal,
                    DataCriacao = l.DataCriacao
                }).ToList();
        }

        public List<ConciliacaoBancariaDTO> SugerirConciliacao(int empresaId, int contaId)
        {
            var conta = _context.ContasBancarias.Find(contaId)
                ?? throw new KeyNotFoundException("Conta não encontrada.");

            var empresa = _context.Empresas.Find(empresaId);
            var diasTolerance = 3;

            var manuais = _context.LancamentosBancarios
                .Where(l => l.IdEmpresa == empresaId && l.IdContaBancaria == contaId
                         && l.Origem == "MANUAL" && l.Status == "PENDENTE")
                .ToList();

            var importados = _context.LancamentosBancarios
                .Where(l => l.IdEmpresa == empresaId && l.IdContaBancaria == contaId
                         && l.Origem == "IMPORTADO" && l.Status == "PENDENTE")
                .ToList();

            var sugestoes = new List<ConciliacaoBancariaDTO>();
            foreach (var m in manuais)
            {
                var match = importados.FirstOrDefault(i =>
                    i.Tipo == m.Tipo && i.Valor == m.Valor &&
                    Math.Abs((i.DataMovimentacao - m.DataMovimentacao).Days) <= diasTolerance);

                if (match != null)
                {
                    sugestoes.Add(new ConciliacaoBancariaDTO
                    {
                        IdEmpresa = empresaId,
                        IdLancamentoManual = m.Id,
                        IdLancamentoImportado = match.Id,
                        Tipo = "SUGERIDO",
                        Status = "SUGERIDO",
                        DataCriacao = DateTime.Now
                    });
                }
            }
            return sugestoes;
        }

        public ConciliacaoBancaria Conciliar(ConciliarModel model)
        {
            var manual = _context.LancamentosBancarios.Find(model.IdLancamentoManual)
                ?? throw new KeyNotFoundException("Lançamento manual não encontrado.");
            var importado = _context.LancamentosBancarios.Find(model.IdLancamentoImportado)
                ?? throw new KeyNotFoundException("Lançamento importado não encontrado.");

            manual.Status = "CONCILIADO";
            manual.DataAtualizacao = DateTime.Now;
            importado.Status = "CONCILIADO";
            importado.DataAtualizacao = DateTime.Now;

            var conciliacao = new ConciliacaoBancaria
            {
                IdEmpresa = model.IdEmpresa,
                IdLancamentoManual = model.IdLancamentoManual,
                IdLancamentoImportado = model.IdLancamentoImportado,
                Tipo = "CONCILIADO_MANUAL",
                Status = "CONCILIADO_MANUAL",
                Observacao = model.Observacao,
                IdResponsavel = model.IdResponsavel
            };
            _context.ConciliacoesBancarias.Add(conciliacao);
            _context.SaveChanges();
            return conciliacao;
        }

        public void Desconciliar(DesconciliarModel model)
        {
            var conc = _context.ConciliacoesBancarias.Find(model.IdConciliacao)
                ?? throw new KeyNotFoundException("Conciliação não encontrada.");

            if (conc.IdLancamentoManual.HasValue)
            {
                var m = _context.LancamentosBancarios.Find(conc.IdLancamentoManual.Value);
                if (m != null) { m.Status = "PENDENTE"; m.DataAtualizacao = DateTime.Now; }
            }
            if (conc.IdLancamentoImportado.HasValue)
            {
                var i = _context.LancamentosBancarios.Find(conc.IdLancamentoImportado.Value);
                if (i != null) { i.Status = "PENDENTE"; i.DataAtualizacao = DateTime.Now; }
            }

            conc.Status = "IGNORADO";
            _context.SaveChanges();
        }
    }

    public class FechamentoBancarioService : IFechamentoBancarioService
    {
        private readonly ContextLocalContext _context;

        public FechamentoBancarioService(ContextLocalContext context) => _context = context;

        public List<FechamentoBancarioDTO> Listar(int empresaId)
        {
            return _context.FechamentosBancarios
                .Include(f => f.ContaBancaria)
                .Where(f => f.IdEmpresa == empresaId)
                .OrderByDescending(f => f.Ano).ThenByDescending(f => f.Mes)
                .Select(f => new FechamentoBancarioDTO
                {
                    Id = f.Id, IdEmpresa = f.IdEmpresa, IdContaBancaria = f.IdContaBancaria,
                    NomeConta = f.ContaBancaria != null ? f.ContaBancaria.Nome : null,
                    Ano = f.Ano, Mes = f.Mes,
                    SaldoInicial = f.SaldoInicial, TotalCreditos = f.TotalCreditos,
                    TotalDebitos = f.TotalDebitos, SaldoFinal = f.SaldoFinal,
                    QtdLancamentos = f.QtdLancamentos, QtdConciliados = f.QtdConciliados,
                    QtdPendentes = f.QtdPendentes, Status = f.Status,
                    DataFechamento = f.DataFechamento
                }).ToList();
        }

        public FechamentoBancarioDTO Obter(int empresaId, int contaId, int ano, int mes)
        {
            var f = _context.FechamentosBancarios
                .Include(x => x.ContaBancaria)
                .FirstOrDefault(x => x.IdEmpresa == empresaId && x.IdContaBancaria == contaId
                                  && x.Ano == ano && x.Mes == mes)
                ?? throw new KeyNotFoundException($"Fechamento {ano}/{mes:D2} não encontrado.");

            return new FechamentoBancarioDTO
            {
                Id = f.Id, IdEmpresa = f.IdEmpresa, IdContaBancaria = f.IdContaBancaria,
                NomeConta = f.ContaBancaria?.Nome,
                Ano = f.Ano, Mes = f.Mes,
                SaldoInicial = f.SaldoInicial, TotalCreditos = f.TotalCreditos,
                TotalDebitos = f.TotalDebitos, SaldoFinal = f.SaldoFinal,
                QtdLancamentos = f.QtdLancamentos, QtdConciliados = f.QtdConciliados,
                QtdPendentes = f.QtdPendentes, Status = f.Status, DataFechamento = f.DataFechamento
            };
        }

        public FechamentoBancarioMensal Fechar(FecharMesModel model)
        {
            var existe = _context.FechamentosBancarios
                .Any(f => f.IdContaBancaria == model.IdContaBancaria
                       && f.Ano == model.Ano && f.Mes == model.Mes);
            if (existe)
                throw new InvalidOperationException($"Mês {model.Ano}/{model.Mes:D2} já está fechado.");

            var conta = _context.ContasBancarias.Find(model.IdContaBancaria)
                ?? throw new KeyNotFoundException("Conta não encontrada.");

            var dataInicio = new DateTime(model.Ano, model.Mes, 1);
            var dataFim = dataInicio.AddMonths(1).AddDays(-1);

            var lancamentos = _context.LancamentosBancarios
                .Where(l => l.IdContaBancaria == model.IdContaBancaria
                         && l.DataMovimentacao >= dataInicio
                         && l.DataMovimentacao <= dataFim
                         && l.Status != "CANCELADO")
                .ToList();

            var pendentes = lancamentos.Count(l => l.Status == "PENDENTE");
            var empresa = _context.Empresas.Find(model.IdEmpresa);

            // Calcular saldo inicial = saldo inicial da conta + tudo antes do período
            var lancAnteriores = _context.LancamentosBancarios
                .Where(l => l.IdContaBancaria == model.IdContaBancaria
                         && l.DataMovimentacao < dataInicio && l.Status != "CANCELADO")
                .ToList();

            var saldoInicial = conta.SaldoInicial
                + lancAnteriores.Where(l => l.Tipo == "CREDITO").Sum(l => l.Valor)
                - lancAnteriores.Where(l => l.Tipo == "DEBITO").Sum(l => l.Valor);

            var totalCreditos = lancamentos.Where(l => l.Tipo == "CREDITO").Sum(l => l.Valor);
            var totalDebitos = lancamentos.Where(l => l.Tipo == "DEBITO").Sum(l => l.Valor);

            var entity = new FechamentoBancarioMensal
            {
                IdEmpresa = model.IdEmpresa,
                IdContaBancaria = model.IdContaBancaria,
                Ano = model.Ano,
                Mes = model.Mes,
                SaldoInicial = saldoInicial,
                TotalCreditos = totalCreditos,
                TotalDebitos = totalDebitos,
                SaldoFinal = saldoInicial + totalCreditos - totalDebitos,
                QtdLancamentos = lancamentos.Count,
                QtdConciliados = lancamentos.Count(l => l.Status == "CONCILIADO"),
                QtdPendentes = pendentes,
                Status = "FECHADO",
                IdFechadoPor = model.IdResponsavel,
                DataFechamento = DateTime.Now
            };

            _context.FechamentosBancarios.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Reabrir(int id, int idResponsavel)
        {
            var entity = _context.FechamentosBancarios.Find(id)
                ?? throw new KeyNotFoundException($"Fechamento {id} não encontrado.");

            if (entity.Status != "FECHADO")
                throw new InvalidOperationException("Fechamento já está aberto.");

            entity.Status = "ABERTO";
            entity.IdReabertoPor = idResponsavel;
            entity.DataReabertura = DateTime.Now;
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }
    }

    public class RegraClassificacaoService : IRegraClassificacaoService
    {
        private readonly ContextLocalContext _context;

        public RegraClassificacaoService(ContextLocalContext context) => _context = context;

        public List<RegraClassificacaoDTO> Listar(int empresaId)
        {
            return _context.RegrasClassificacaoBancaria
                .Include(r => r.Categoria)
                .Where(r => r.IdEmpresa == empresaId)
                .OrderBy(r => r.Prioridade)
                .Select(r => new RegraClassificacaoDTO
                {
                    Id = r.Id, IdEmpresa = r.IdEmpresa, IdCategoria = r.IdCategoria,
                    NomeCategoria = r.Categoria != null ? r.Categoria.Nome : null,
                    PalavraChave = r.PalavraChave, TipoLancamento = r.TipoLancamento,
                    Prioridade = r.Prioridade, Status = r.Status
                }).ToList();
        }

        public RegraClassificacaoBancaria Criar(RegraClassificacaoSalvarModel model)
        {
            var entity = new RegraClassificacaoBancaria
            {
                IdEmpresa = model.IdEmpresa,
                IdCategoria = model.IdCategoria,
                PalavraChave = model.PalavraChave.Trim().ToUpperInvariant(),
                TipoLancamento = model.TipoLancamento,
                Prioridade = model.Prioridade,
                Status = "ATIVA"
            };
            _context.RegrasClassificacaoBancaria.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public RegraClassificacaoBancaria Alterar(int id, RegraClassificacaoSalvarModel model)
        {
            var entity = _context.RegrasClassificacaoBancaria.Find(id)
                ?? throw new KeyNotFoundException($"Regra {id} não encontrada.");

            entity.IdCategoria = model.IdCategoria;
            entity.PalavraChave = model.PalavraChave.Trim().ToUpperInvariant();
            entity.TipoLancamento = model.TipoLancamento;
            entity.Prioridade = model.Prioridade;
            entity.Status = model.Status;
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
            return entity;
        }

        public void Deletar(int id)
        {
            var entity = _context.RegrasClassificacaoBancaria.Find(id)
                ?? throw new KeyNotFoundException($"Regra {id} não encontrada.");
            _context.RegrasClassificacaoBancaria.Remove(entity);
            _context.SaveChanges();
        }
    }

    public class DashboardBancarioService : IDashboardBancarioService
    {
        private readonly ContextLocalContext _context;

        public DashboardBancarioService(ContextLocalContext context) => _context = context;

        public DashboardBancarioDTO ObterDashboard(int empresaId, int ano, int mes)
        {
            var contas = _context.ContasBancarias
                .Where(c => c.IdEmpresa == empresaId && c.Status == "ATIVA")
                .ToList();

            var contaIds = contas.Select(c => c.Id).ToList();
            var todosLanc = _context.LancamentosBancarios.ToList()
                .Where(l => l.IdEmpresa == empresaId && contaIds.Contains(l.IdContaBancaria) && l.Status != "CANCELADO")
                .ToList();

            var dataInicio = new DateTime(ano, mes, 1);
            var dataFim = dataInicio.AddMonths(1).AddDays(-1);
            var lancMes = todosLanc.Where(l => l.DataMovimentacao >= dataInicio && l.DataMovimentacao <= dataFim).ToList();

            var saldoContas = contas.Select(c =>
            {
                var lancConta = todosLanc.Where(l => l.IdContaBancaria == c.Id).ToList();
                var cr = lancConta.Where(l => l.Tipo == "CREDITO").Sum(l => l.Valor);
                var db = lancConta.Where(l => l.Tipo == "DEBITO").Sum(l => l.Valor);
                return new SaldoContaDTO
                {
                    IdContaBancaria = c.Id, NomeConta = c.Nome,
                    SaldoInicial = c.SaldoInicial,
                    TotalCreditos = cr, TotalDebitos = db,
                    SaldoAtual = c.SaldoInicial + cr - db
                };
            }).ToList();

            return new DashboardBancarioDTO
            {
                SaldoTotalEmpresa = saldoContas.Sum(s => s.SaldoAtual),
                CreditosMes = lancMes.Where(l => l.Tipo == "CREDITO").Sum(l => l.Valor),
                DebitosMes = lancMes.Where(l => l.Tipo == "DEBITO").Sum(l => l.Valor),
                ResultadoLiquidoMes = lancMes.Where(l => l.Tipo == "CREDITO").Sum(l => l.Valor)
                                   - lancMes.Where(l => l.Tipo == "DEBITO").Sum(l => l.Valor),
                LancamentosPendentesConciliacao = todosLanc.Count(l => l.Status == "PENDENTE"),
                SaldoPorConta = saldoContas
            };
        }
    }
}
