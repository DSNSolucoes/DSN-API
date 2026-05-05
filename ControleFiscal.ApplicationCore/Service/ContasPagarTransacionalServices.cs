using ControleFiscal.Domain.DTO.ContasPagar;
using ControleFiscal.Domain.Model.ContasPagar;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleFiscal.ApplicationCore.Service
{
    public class ContaPagarService : IContaPagarService
    {
        private readonly ContextLocalContext _context;
        public ContaPagarService(ContextLocalContext context) => _context = context;

        public List<ContaPagarDTO> Listar(FiltroContasPagarModel filtro)
        {
            var query = _context.ContasPagar
                .Include(c => c.Categoria)
                .Include(c => c.CentroCusto)
                .Where(c => c.IdEmpresa == filtro.IdEmpresa);

            if (filtro.IdFornecedor.HasValue)
                query = query.Where(c => c.IdFornecedor == filtro.IdFornecedor);
            if (filtro.IdCategoria.HasValue)
                query = query.Where(c => c.IdCategoria == filtro.IdCategoria);
            if (filtro.IdCentroCusto.HasValue)
                query = query.Where(c => c.IdCentroCusto == filtro.IdCentroCusto);
            if (!string.IsNullOrWhiteSpace(filtro.Status))
                query = query.Where(c => c.Status == filtro.Status);
            if (filtro.VencimentoDe.HasValue)
                query = query.Where(c => c.DataVencimento >= filtro.VencimentoDe.Value);
            if (filtro.VencimentoAte.HasValue)
                query = query.Where(c => c.DataVencimento <= filtro.VencimentoAte.Value);
            if (!string.IsNullOrWhiteSpace(filtro.NumeroDocumento))
                query = query.Where(c => c.NumeroDocumento != null && c.NumeroDocumento.Contains(filtro.NumeroDocumento));
            if (!string.IsNullOrWhiteSpace(filtro.Descricao))
                query = query.Where(c => c.Descricao.Contains(filtro.Descricao));

            var contas = query
                .OrderBy(c => c.DataVencimento)
                .Skip((filtro.Pagina - 1) * filtro.TamanhoPagina)
                .Take(filtro.TamanhoPagina)
                .ToList();

            var fornecedores = _context.Fornecedores
                .Where(f => f.Id_Empresa == filtro.IdEmpresa)
                .ToDictionary(f => f.CdFornecedor, f => f.NmFornecedor);

            var hoje = DateTime.Today;
            return contas.Select(c => new ContaPagarDTO
            {
                Id = c.Id,
                IdEmpresa = c.IdEmpresa,
                IdFornecedor = c.IdFornecedor,
                NomeFornecedor = c.IdFornecedor.HasValue && fornecedores.TryGetValue(c.IdFornecedor.Value, out var nf) ? nf : null,
                IdCategoria = c.IdCategoria,
                NomeCategoria = c.Categoria?.Nome,
                IdCentroCusto = c.IdCentroCusto,
                NomeCentroCusto = c.CentroCusto?.Nome,
                Descricao = c.Descricao,
                NumeroDocumento = c.NumeroDocumento,
                TipoDocumento = c.TipoDocumento,
                DataEmissao = c.DataEmissao,
                DataCompetencia = c.DataCompetencia,
                DataVencimento = c.DataVencimento,
                ValorOriginal = c.ValorOriginal,
                ValorDesconto = c.ValorDesconto,
                ValorAcrescimo = c.ValorAcrescimo,
                ValorMulta = c.ValorMulta,
                ValorJuros = c.ValorJuros,
                ValorTotal = c.ValorTotal,
                ValorPago = c.ValorPago,
                SaldoAPagar = c.SaldoAPagar,
                NumeroParcela = c.NumeroParcela,
                TotalParcelas = c.TotalParcelas,
                IdContaOrigem = c.IdContaOrigem,
                Recorrente = c.Recorrente,
                Status = c.Status,
                Observacoes = c.Observacoes,
                DataCriacao = c.DataCriacao,
                DiasAtraso = (c.Status == "VENCIDA" || c.Status == "ABERTA") && c.DataVencimento < hoje
                    ? (hoje - c.DataVencimento).Days : 0
            }).ToList();
        }

        public ContaPagarDTO Obter(int empresaId, int id)
        {
            var hoje = DateTime.Today;
            var c = _context.ContasPagar
                .Include(x => x.Categoria)
                .Include(x => x.CentroCusto)
                .FirstOrDefault(x => x.Id == id && x.IdEmpresa == empresaId)
                ?? throw new KeyNotFoundException($"Conta a pagar {id} não encontrada.");

            string? nomeFornecedor = null;
            if (c.IdFornecedor.HasValue)
                nomeFornecedor = _context.Fornecedores
                    .FirstOrDefault(f => f.CdFornecedor == c.IdFornecedor.Value && f.Id_Empresa == empresaId)
                    ?.NmFornecedor;

            return new ContaPagarDTO
            {
                Id = c.Id, IdEmpresa = c.IdEmpresa, IdFornecedor = c.IdFornecedor,
                NomeFornecedor = nomeFornecedor, IdCategoria = c.IdCategoria,
                NomeCategoria = c.Categoria?.Nome, IdCentroCusto = c.IdCentroCusto,
                NomeCentroCusto = c.CentroCusto?.Nome, Descricao = c.Descricao,
                NumeroDocumento = c.NumeroDocumento, TipoDocumento = c.TipoDocumento,
                DataEmissao = c.DataEmissao, DataCompetencia = c.DataCompetencia,
                DataVencimento = c.DataVencimento, ValorOriginal = c.ValorOriginal,
                ValorDesconto = c.ValorDesconto, ValorAcrescimo = c.ValorAcrescimo,
                ValorMulta = c.ValorMulta, ValorJuros = c.ValorJuros,
                ValorTotal = c.ValorTotal, ValorPago = c.ValorPago,
                SaldoAPagar = c.SaldoAPagar, NumeroParcela = c.NumeroParcela,
                TotalParcelas = c.TotalParcelas, IdContaOrigem = c.IdContaOrigem,
                Recorrente = c.Recorrente, Status = c.Status,
                Observacoes = c.Observacoes, DataCriacao = c.DataCriacao,
                DiasAtraso = c.DataVencimento < hoje && c.SaldoAPagar > 0
                    ? (hoje - c.DataVencimento).Days : 0
            };
        }

        public ContaPagar Criar(ContaPagarSalvarModel model)
        {
            if (model.ValorOriginal <= 0)
                throw new ArgumentException("Informe um valor maior que zero.");

            if (string.IsNullOrWhiteSpace(model.Descricao))
                throw new ArgumentException("Descrição é obrigatória.");

            if (model.DataVencimento == default)
                throw new ArgumentException("Data de vencimento é obrigatória.");

            var total = model.ValorOriginal - model.ValorDesconto + model.ValorAcrescimo + model.ValorMulta + model.ValorJuros;
            var entity = new ContaPagar
            {
                IdEmpresa = model.IdEmpresa,
                IdFornecedor = model.IdFornecedor,
                IdCategoria = model.IdCategoria,
                IdCentroCusto = model.IdCentroCusto,
                Descricao = model.Descricao.Trim(),
                NumeroDocumento = model.NumeroDocumento?.Trim(),
                SerieDocumento = model.SerieDocumento?.Trim(),
                TipoDocumento = model.TipoDocumento,
                DataEmissao = model.DataEmissao,
                DataCompetencia = model.DataCompetencia,
                DataVencimento = model.DataVencimento,
                ValorOriginal = model.ValorOriginal,
                ValorDesconto = model.ValorDesconto,
                ValorAcrescimo = model.ValorAcrescimo,
                ValorMulta = model.ValorMulta,
                ValorJuros = model.ValorJuros,
                ValorTotal = total,
                ValorPago = 0,
                SaldoAPagar = total,
                NumeroParcela = 1,
                TotalParcelas = 1,
                Status = "ABERTA",
                Observacoes = model.Observacoes,
                IdResponsavelCriacao = model.IdResponsavel,
                DataCriacao = DateTime.Now
            };
            _context.ContasPagar.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public ContaPagar Alterar(int id, ContaPagarSalvarModel model)
        {
            var entity = _context.ContasPagar
                .FirstOrDefault(x => x.Id == id && x.IdEmpresa == model.IdEmpresa)
                ?? throw new KeyNotFoundException($"Conta {id} não encontrada.");

            if (entity.Status == "CANCELADA")
                throw new InvalidOperationException("Não é possível alterar uma conta cancelada.");

            var total = model.ValorOriginal - model.ValorDesconto + model.ValorAcrescimo + model.ValorMulta + model.ValorJuros;

            entity.IdFornecedor = model.IdFornecedor;
            entity.IdCategoria = model.IdCategoria;
            entity.IdCentroCusto = model.IdCentroCusto;
            entity.Descricao = model.Descricao.Trim();
            entity.NumeroDocumento = model.NumeroDocumento?.Trim();
            entity.SerieDocumento = model.SerieDocumento?.Trim();
            entity.TipoDocumento = model.TipoDocumento;
            entity.DataEmissao = model.DataEmissao;
            entity.DataCompetencia = model.DataCompetencia;
            entity.DataVencimento = model.DataVencimento;
            entity.ValorOriginal = model.ValorOriginal;
            entity.ValorDesconto = model.ValorDesconto;
            entity.ValorAcrescimo = model.ValorAcrescimo;
            entity.ValorMulta = model.ValorMulta;
            entity.ValorJuros = model.ValorJuros;
            entity.ValorTotal = total;
            entity.SaldoAPagar = total - entity.ValorPago;
            entity.Observacoes = model.Observacoes;
            entity.IdResponsavelAtualizacao = model.IdResponsavel;
            entity.DataAtualizacao = DateTime.Now;

            AtualizarStatus(entity);
            _context.SaveChanges();
            return entity;
        }

        public List<ContaPagar> GerarParcelas(GerarParcelasModel model)
        {
            if (model.NumeroParcelas < 2 || model.NumeroParcelas > 60)
                throw new ArgumentException("Número de parcelas deve ser entre 2 e 60.");

            var valorParcela = Math.Round(model.ValorTotal / model.NumeroParcelas, 2);
            var diferenca = model.ValorTotal - (valorParcela * model.NumeroParcelas);

            var parcelas = new List<ContaPagar>();
            for (int i = 1; i <= model.NumeroParcelas; i++)
            {
                var valor = i == model.NumeroParcelas ? valorParcela + diferenca : valorParcela;
                var vencimento = model.DataPrimeiroVencimento.AddMonths(i - 1);
                var parcela = new ContaPagar
                {
                    IdEmpresa = model.IdEmpresa,
                    IdFornecedor = model.IdFornecedor,
                    IdCategoria = model.IdCategoria,
                    IdCentroCusto = model.IdCentroCusto,
                    Descricao = $"{model.Descricao.Trim()} ({i}/{model.NumeroParcelas})",
                    NumeroDocumento = model.NumeroDocumento?.Trim(),
                    TipoDocumento = model.TipoDocumento,
                    DataEmissao = model.DataEmissao,
                    DataVencimento = vencimento,
                    ValorOriginal = valor,
                    ValorTotal = valor,
                    SaldoAPagar = valor,
                    NumeroParcela = i,
                    TotalParcelas = model.NumeroParcelas,
                    Status = "ABERTA",
                    Observacoes = model.Observacoes,
                    IdResponsavelCriacao = model.IdResponsavel,
                    DataCriacao = DateTime.Now
                };
                parcelas.Add(parcela);
            }

            // Vincula contaOrigem com a primeira
            _context.ContasPagar.AddRange(parcelas);
            _context.SaveChanges();

            // Atualiza IdContaOrigem das demais parcelas
            var idOrigem = parcelas[0].Id;
            foreach (var p in parcelas)
            {
                p.IdContaOrigem = idOrigem;
            }
            _context.SaveChanges();

            return parcelas;
        }

        public void Cancelar(int id, CancelarContaModel model)
        {
            var entity = _context.ContasPagar
                .FirstOrDefault(x => x.Id == id && x.IdEmpresa == model.IdEmpresa)
                ?? throw new KeyNotFoundException($"Conta {id} não encontrada.");

            if (entity.Status == "CANCELADA")
                throw new InvalidOperationException("Conta já está cancelada.");
            if (entity.Status == "PAGA")
                throw new InvalidOperationException("Não é possível cancelar uma conta totalmente paga.");

            entity.Status = "CANCELADA";
            entity.MotivoCancelamento = model.MotivoCancelamento;
            entity.IdResponsavelCancelamento = model.IdResponsavel;
            entity.DataCancelamento = DateTime.Now;
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }

        public void Reabrir(int id, ReobrirContaModel model)
        {
            var entity = _context.ContasPagar
                .FirstOrDefault(x => x.Id == id && x.IdEmpresa == model.IdEmpresa)
                ?? throw new KeyNotFoundException($"Conta {id} não encontrada.");

            if (entity.Status == "CANCELADA")
                throw new InvalidOperationException("Conta cancelada não pode ser reaberta. Use a opção de cancelamento.");

            if (entity.Status == "ABERTA" || entity.Status == "VENCIDA")
                throw new InvalidOperationException("Conta já está aberta.");

            // Estorna todos os pagamentos ativos e zera os valores
            var pagamentos = _context.PagamentosContaPagar
                .Where(p => p.IdContaPagar == id && p.Status != "ESTORNADO")
                .ToList();

            foreach (var p in pagamentos)
            {
                p.Status = "ESTORNADO";
                p.DataEstorno = DateTime.Now;
                p.MotivoEstorno = "Conta reaberta manualmente";
            }

            entity.ValorPago = 0;
            entity.SaldoAPagar = entity.ValorTotal;
            entity.IdResponsavelAtualizacao = model.IdResponsavel;
            entity.DataAtualizacao = DateTime.Now;
            AtualizarStatus(entity);
            _context.SaveChanges();
        }

        public void AtualizarVencidas(int empresaId)
        {
            var hoje = DateTime.Today;
            var contasAbertas = _context.ContasPagar
                .Where(c => c.IdEmpresa == empresaId
                    && c.Status == "ABERTA"
                    && c.DataVencimento < hoje)
                .ToList();

            foreach (var c in contasAbertas)
                c.Status = "VENCIDA";

            if (contasAbertas.Count > 0)
                _context.SaveChanges();
        }

        private static void AtualizarStatus(ContaPagar c)
        {
            if (c.SaldoAPagar <= 0)
                c.Status = "PAGA";
            else if (c.ValorPago > 0)
                c.Status = "PAGA_PARCIAL";
            else if (c.DataVencimento < DateTime.Today)
                c.Status = "VENCIDA";
            else
                c.Status = "ABERTA";
        }
    }

    public class PagamentoContaPagarService : IPagamentoContaPagarService
    {
        private readonly ContextLocalContext _context;
        public PagamentoContaPagarService(ContextLocalContext context) => _context = context;

        public List<PagamentoContaPagarDTO> ListarPorConta(int empresaId, int contaPagarId)
        {
            return _context.PagamentosContaPagar
                .Where(p => p.IdEmpresa == empresaId && p.IdContaPagar == contaPagarId)
                .OrderByDescending(p => p.DataCriacao)
                .Select(p => new PagamentoContaPagarDTO
                {
                    Id = p.Id,
                    IdEmpresa = p.IdEmpresa,
                    IdContaPagar = p.IdContaPagar,
                    DataPagamento = p.DataPagamento,
                    ValorPago = p.ValorPago,
                    ValorDesconto = p.ValorDesconto,
                    ValorJuros = p.ValorJuros,
                    ValorMulta = p.ValorMulta,
                    FormaPagamento = p.FormaPagamento,
                    DocumentoPagamento = p.DocumentoPagamento,
                    Status = p.Status,
                    Observacoes = p.Observacoes,
                    DataCriacao = p.DataCriacao,
                    DataEstorno = p.DataEstorno,
                    MotivoEstorno = p.MotivoEstorno
                }).ToList();
        }

        public PagamentoContaPagar Registrar(int contaPagarId, RegistrarPagamentoModel model)
        {
            var conta = _context.ContasPagar
                .FirstOrDefault(c => c.Id == contaPagarId && c.IdEmpresa == model.IdEmpresa)
                ?? throw new KeyNotFoundException($"Conta {contaPagarId} não encontrada.");

            if (conta.Status == "CANCELADA")
                throw new InvalidOperationException("Não é possível registrar pagamento em conta cancelada.");
            if (conta.Status == "PAGA")
                throw new InvalidOperationException("Conta já está totalmente paga.");
            if (model.ValorPago <= 0)
                throw new ArgumentException("Valor do pagamento deve ser maior que zero.");
            if (model.ValorPago > conta.SaldoAPagar)
                throw new ArgumentException("Valor do pagamento não pode ser maior que o saldo a pagar.");

            var pagamento = new PagamentoContaPagar
            {
                IdEmpresa = model.IdEmpresa,
                IdContaPagar = contaPagarId,
                DataPagamento = model.DataPagamento,
                ValorPago = model.ValorPago,
                ValorDesconto = model.ValorDesconto,
                ValorJuros = model.ValorJuros,
                ValorMulta = model.ValorMulta,
                FormaPagamento = model.FormaPagamento,
                DocumentoPagamento = model.DocumentoPagamento,
                Observacoes = model.Observacoes,
                Status = "ATIVO",
                IdResponsavel = model.IdResponsavel,
                DataCriacao = DateTime.Now
            };
            _context.PagamentosContaPagar.Add(pagamento);
            _context.SaveChanges();

            RecalcularConta(model.IdEmpresa, contaPagarId);
            return pagamento;
        }

        public void Estornar(int pagamentoId, EstornarPagamentoModel model)
        {
            var pagamento = _context.PagamentosContaPagar
                .FirstOrDefault(p => p.Id == pagamentoId && p.IdEmpresa == model.IdEmpresa)
                ?? throw new KeyNotFoundException($"Pagamento {pagamentoId} não encontrado.");

            if (pagamento.Status == "ESTORNADO")
                throw new InvalidOperationException("Pagamento já foi estornado.");

            pagamento.Status = "ESTORNADO";
            pagamento.MotivoEstorno = model.MotivoEstorno;
            pagamento.IdResponsavelEstorno = model.IdResponsavel;
            pagamento.DataEstorno = DateTime.Now;
            _context.SaveChanges();

            RecalcularConta(model.IdEmpresa, pagamento.IdContaPagar);
        }

        private void RecalcularConta(int empresaId, int contaPagarId)
        {
            var conta = _context.ContasPagar.Find(contaPagarId);
            if (conta == null) return;

            var totalPago = _context.PagamentosContaPagar
                .Where(p => p.IdEmpresa == empresaId && p.IdContaPagar == contaPagarId && p.Status == "ATIVO")
                .Sum(p => (decimal?)p.ValorPago) ?? 0m;

            conta.ValorPago = totalPago;
            conta.SaldoAPagar = conta.ValorTotal - totalPago;

            if (conta.SaldoAPagar <= 0)
                conta.Status = "PAGA";
            else if (totalPago > 0)
                conta.Status = "PAGA_PARCIAL";
            else if (conta.DataVencimento < DateTime.Today)
                conta.Status = "VENCIDA";
            else
                conta.Status = "ABERTA";

            conta.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }
    }

    public class ContaPagarRecorrenteService : IContaPagarRecorrenteService
    {
        private readonly ContextLocalContext _context;
        public ContaPagarRecorrenteService(ContextLocalContext context) => _context = context;

        public List<ContaPagarRecorrenteDTO> Listar(int empresaId)
        {
            var recorrentes = _context.ContasPagarRecorrentes
                .Include(r => r.Categoria)
                .Where(r => r.IdEmpresa == empresaId)
                .OrderBy(r => r.Descricao)
                .ToList();

            var fornecedores = _context.Fornecedores
                .Where(f => f.Id_Empresa == empresaId)
                .ToDictionary(f => f.CdFornecedor, f => f.NmFornecedor);

            return recorrentes.Select(r => new ContaPagarRecorrenteDTO
            {
                Id = r.Id,
                IdEmpresa = r.IdEmpresa,
                IdFornecedor = r.IdFornecedor,
                NomeFornecedor = r.IdFornecedor.HasValue && fornecedores.TryGetValue(r.IdFornecedor.Value, out var nf) ? nf : null,
                IdCategoria = r.IdCategoria,
                NomeCategoria = r.Categoria?.Nome,
                Descricao = r.Descricao,
                Valor = r.Valor,
                Periodicidade = r.Periodicidade,
                DiaVencimento = r.DiaVencimento,
                DataInicio = r.DataInicio,
                DataFim = r.DataFim,
                Status = r.Status
            }).ToList();
        }

        public ContaPagarRecorrenteDTO Obter(int empresaId, int id)
        {
            var r = _context.ContasPagarRecorrentes
                .Include(x => x.Categoria)
                .FirstOrDefault(x => x.Id == id && x.IdEmpresa == empresaId)
                ?? throw new KeyNotFoundException($"Recorrência {id} não encontrada.");

            string? nomeFornecedor = null;
            if (r.IdFornecedor.HasValue)
                nomeFornecedor = _context.Fornecedores
                    .FirstOrDefault(f => f.CdFornecedor == r.IdFornecedor.Value && f.Id_Empresa == empresaId)
                    ?.NmFornecedor;

            return new ContaPagarRecorrenteDTO
            {
                Id = r.Id, IdEmpresa = r.IdEmpresa, IdFornecedor = r.IdFornecedor,
                NomeFornecedor = nomeFornecedor, IdCategoria = r.IdCategoria,
                NomeCategoria = r.Categoria?.Nome, Descricao = r.Descricao,
                Valor = r.Valor, Periodicidade = r.Periodicidade,
                DiaVencimento = r.DiaVencimento, DataInicio = r.DataInicio,
                DataFim = r.DataFim, Status = r.Status
            };
        }

        public ContaPagarRecorrente Criar(ContaPagarRecorrenteSalvarModel model)
        {
            var entity = new ContaPagarRecorrente
            {
                IdEmpresa = model.IdEmpresa,
                IdFornecedor = model.IdFornecedor,
                IdCategoria = model.IdCategoria,
                IdCentroCusto = model.IdCentroCusto,
                Descricao = model.Descricao.Trim(),
                Valor = model.Valor,
                Periodicidade = model.Periodicidade,
                DiaVencimento = model.DiaVencimento,
                DataInicio = model.DataInicio,
                DataFim = model.DataFim,
                Status = "ATIVA",
                Observacoes = model.Observacoes,
                DataCriacao = DateTime.Now
            };
            _context.ContasPagarRecorrentes.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public ContaPagarRecorrente Alterar(int id, ContaPagarRecorrenteSalvarModel model)
        {
            var entity = _context.ContasPagarRecorrentes
                .FirstOrDefault(x => x.Id == id && x.IdEmpresa == model.IdEmpresa)
                ?? throw new KeyNotFoundException($"Recorrência {id} não encontrada.");

            entity.IdFornecedor = model.IdFornecedor;
            entity.IdCategoria = model.IdCategoria;
            entity.IdCentroCusto = model.IdCentroCusto;
            entity.Descricao = model.Descricao.Trim();
            entity.Valor = model.Valor;
            entity.Periodicidade = model.Periodicidade;
            entity.DiaVencimento = model.DiaVencimento;
            entity.DataFim = model.DataFim;
            entity.Observacoes = model.Observacoes;
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
            return entity;
        }

        public void Cancelar(int id, int? idResponsavel)
        {
            var entity = _context.ContasPagarRecorrentes.Find(id)
                ?? throw new KeyNotFoundException($"Recorrência {id} não encontrada.");
            entity.Status = "CANCELADA";
            entity.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
        }

        public List<ContaPagar> GerarContas(int id, GerarContasRecorrentesModel model)
        {
            var recorrencia = _context.ContasPagarRecorrentes
                .FirstOrDefault(r => r.Id == id && r.IdEmpresa == model.IdEmpresa)
                ?? throw new KeyNotFoundException($"Recorrência {id} não encontrada.");

            if (recorrencia.Status != "ATIVA")
                throw new InvalidOperationException("Recorrência não está ativa.");

            var geradas = new List<ContaPagar>();
            var dataAtual = recorrencia.DataInicio;
            var diaVencimento = recorrencia.DiaVencimento ?? dataAtual.Day;

            while (dataAtual <= model.DataLimite)
            {
                if (recorrencia.DataFim.HasValue && dataAtual > recorrencia.DataFim.Value)
                    break;

                var vencimento = new DateTime(dataAtual.Year, dataAtual.Month,
                    Math.Min(diaVencimento, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month)));

                // Evita duplicar conta já gerada para este vencimento
                var jaExiste = _context.ContasPagar.Any(c =>
                    c.IdRecorrencia == recorrencia.Id && c.DataVencimento == vencimento);

                if (!jaExiste)
                {
                    var conta = new ContaPagar
                    {
                        IdEmpresa = recorrencia.IdEmpresa,
                        IdFornecedor = recorrencia.IdFornecedor,
                        IdCategoria = recorrencia.IdCategoria,
                        IdCentroCusto = recorrencia.IdCentroCusto,
                        Descricao = recorrencia.Descricao,
                        DataVencimento = vencimento,
                        ValorOriginal = recorrencia.Valor,
                        ValorTotal = recorrencia.Valor,
                        SaldoAPagar = recorrencia.Valor,
                        Recorrente = 1,
                        IdRecorrencia = recorrencia.Id,
                        Status = "ABERTA",
                        IdResponsavelCriacao = model.IdResponsavel,
                        DataCriacao = DateTime.Now
                    };
                    geradas.Add(conta);
                }

                dataAtual = recorrencia.Periodicidade switch
                {
                    "QUINZENAL" => dataAtual.AddDays(15),
                    "MENSAL" => dataAtual.AddMonths(1),
                    "BIMESTRAL" => dataAtual.AddMonths(2),
                    "TRIMESTRAL" => dataAtual.AddMonths(3),
                    "SEMESTRAL" => dataAtual.AddMonths(6),
                    "ANUAL" => dataAtual.AddYears(1),
                    _ => dataAtual.AddMonths(1)
                };
            }

            if (geradas.Count > 0)
            {
                _context.ContasPagar.AddRange(geradas);
                _context.SaveChanges();
            }

            return geradas;
        }
    }

    public class DashboardContasPagarService : IDashboardContasPagarService
    {
        private readonly ContextLocalContext _context;
        public DashboardContasPagarService(ContextLocalContext context) => _context = context;

        public DashboardContasPagarDTO Obter(int empresaId)
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);
            var sete = hoje.AddDays(7);

            var contas = _context.ContasPagar
                .Include(c => c.Categoria)
                .Where(c => c.IdEmpresa == empresaId && c.Status != "CANCELADA")
                .ToList();

            var fornecedores = _context.Fornecedores
                .Where(f => f.Id_Empresa == empresaId)
                .ToDictionary(f => f.CdFornecedor, f => f.NmFornecedor);

            var emAberto = contas.Where(c => c.Status == "ABERTA" || c.Status == "VENCIDA" || c.Status == "PAGA_PARCIAL").ToList();

            var maioresFornecedores = emAberto
                .Where(c => c.IdFornecedor.HasValue && fornecedores.ContainsKey(c.IdFornecedor.Value))
                .GroupBy(c => fornecedores[c.IdFornecedor!.Value] ?? "Sem nome")
                .Select(g => new ResumoFornecedorDTO
                {
                    NomeFornecedor = g.Key,
                    QtdContas = g.Count(),
                    TotalEmAberto = g.Sum(c => c.SaldoAPagar),
                    TotalPago = g.Sum(c => c.ValorPago)
                })
                .OrderByDescending(f => f.TotalEmAberto)
                .Take(5)
                .ToList();

            var porCategoria = emAberto
                .GroupBy(c => c.Categoria != null ? c.Categoria.Nome : "Sem categoria")
                .Select(g => new ResumoCategoriaDTO
                {
                    NomeCategoria = g.Key,
                    QtdContas = g.Count(),
                    TotalEmAberto = g.Sum(c => c.SaldoAPagar),
                    TotalPago = g.Sum(c => c.ValorPago)
                })
                .OrderByDescending(c => c.TotalEmAberto)
                .Take(8)
                .ToList();

            return new DashboardContasPagarDTO
            {
                TotalEmAberto = emAberto.Sum(c => c.SaldoAPagar),
                TotalVencido = contas.Where(c => c.Status == "VENCIDA").Sum(c => c.SaldoAPagar),
                TotalAVencer = contas.Where(c => c.Status == "ABERTA" && c.DataVencimento <= sete).Sum(c => c.SaldoAPagar),
                TotalPagoMes = contas.Where(c => c.DataAtualizacao >= inicioMes && c.DataAtualizacao <= fimMes
                    && (c.Status == "PAGA" || c.Status == "PAGA_PARCIAL")).Sum(c => c.ValorPago),
                QtdVencidas = contas.Count(c => c.Status == "VENCIDA"),
                QtdAVencer = contas.Count(c => c.Status == "ABERTA" && c.DataVencimento <= sete),
                MaioresFornecedores = maioresFornecedores,
                PorCategoria = porCategoria
            };
        }
    }
}
