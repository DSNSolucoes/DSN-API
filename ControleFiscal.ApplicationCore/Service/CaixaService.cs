using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Services.Interfaces;
using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Domain.Model;
using ControleFiscal.Infrastructure.Sql.Focus.Context;

namespace ControleFiscal.ApplicationCore.Service
{
    public class CaixaService : ICaixaService
    {
        private readonly ContextControleFiscalContext _context;
        private readonly ContextLocalContext _contextLocal;

        public CaixaService(
            ContextControleFiscalContext context,
            ContextLocalContext contextLocal)
        {
            _context = context;
            _contextLocal = contextLocal;
        }
        public List<CaixaDTO> Listar(int lojaId, DateTime data)
        {
            ValidarData(lojaId, data);

            var nomeLoja = _contextLocal.Lojas
                .Where(x => x.Id == lojaId)
                .Select(x => x.Nome)
                .FirstOrDefault();

            var tiposValor = _contextLocal.TipoValorCaixa
                .OrderBy(t => t.Descricao)
                .Select(t => new TipoValor
                {
                    Id = t.Id,
                    Descricao = t.Descricao
                })
                .ToList();

            var caixas = _contextLocal.Caixa
                .Where(c => c.LojaId == lojaId && c.Ativo == "V")
                .OrderBy(c => c.Descricao)
                .ToList();

            var caixaIds = caixas.Select(c => c.Id).ToList();
            var tipoIds = tiposValor.Select(t => t.Id).ToList();

            var movimentacoes = _contextLocal.CaixaMovimentacao
                .ToList()
                .Where(m =>
                    caixaIds.Contains(m.CaixaId) &&
                    tipoIds.Contains(m.TipoValorCaixaId) &&
                    m.Ativo == "V" &&
                    m.DataCompetencia.HasValue &&
                    m.DataCompetencia.Value.Date == data.Date)
                .ToList();

            var retorno = caixas.Select(caixa =>
            {
                var valores = tiposValor.Select(tipo =>
                {
                    var movimentacoesTipo = movimentacoes
                        .Where(m => m.CaixaId == caixa.Id && m.TipoValorCaixaId == tipo.Id)
                        .OrderBy(m => m.DataRealizacao ?? data)
                        .ToList();

                    var detalhes = movimentacoesTipo.Any()
                        ? movimentacoesTipo.Select(m => new CaixaMovimentacaoDetalhesDTO
                        {
                            Id = m.Id,
                            Descricao = m.Descricao ?? string.Empty,
                            Valor = m.Valor,
                            DataCadastro = m.DataCadastro ?? data,
                            DataRealizacao = m.DataRealizacao ?? data,
                            AnoCompetencia = data.Year,
                            MesCompetencia = data.Month,
                            NomeFuncionario = m.NomeFuncionario ?? string.Empty,
                            AnexoNome = m.AnexoNome,
                            AnexoContentType = m.AnexoContentType,
                            AnexoArquivo = m.AnexoArquivo
                        }).ToList()
                        : new List<CaixaMovimentacaoDetalhesDTO>
                        {
                    new CaixaMovimentacaoDetalhesDTO
                    {
                        Id = 0,
                        Descricao = string.Empty,
                        Valor = 0,
                        DataCadastro = data,
                        DataRealizacao = data,
                        AnoCompetencia = data.Year,
                        MesCompetencia = data.Month,
                        NomeFuncionario = string.Empty,
                        AnexoNome = null,
                        AnexoContentType = null,
                        AnexoArquivo = null
                    }
                        };

                    return new CaixaMovimentacoesDTO
                    {
                        Id = movimentacoesTipo.FirstOrDefault()?.Id ?? 0,
                        TipoValorCaixaId = tipo.Id,
                        TipoValorCaixa = tipo,
                        ValorTotal = movimentacoesTipo.Sum(x => x.Valor),
                        Detalhes = detalhes
                    };
                }).ToList();

                return new CaixaDTO
                {
                    Id = caixa.Id,
                    LojaId = caixa.LojaId,
                    NomeLoja = nomeLoja,
                    Descricao = caixa.Descricao,
                    AnoCompetencia = (short)data.Year,
                    MesCompetencia = (short)data.Month,
                    Valores = valores
                };
            }).ToList();

            return retorno;
        }

        public List<CaixaResumoMensalDTO> ListarResumoMensal(int lojaId, int ano, int mes)
        {
            if (lojaId <= 0)
                throw new ArgumentException("LojaId é obrigatório.");

            if (ano <= 0)
                throw new ArgumentException("Ano inválido.");

            if (mes < 1 || mes > 12)
                throw new ArgumentException("Mês inválido.");

            var caixas = _contextLocal.Caixa
                .Where(c => c.LojaId == lojaId && c.Ativo == "V")
                .OrderBy(c => c.Descricao)
                .ToList();

            var caixaIds = caixas.Select(c => c.Id).ToList();

            var movimentacoes = _contextLocal.CaixaMovimentacao
                .ToList()
                .Where(m =>
                    caixaIds.Contains(m.CaixaId) &&
                    m.Ativo == "V" &&
                    m.DataCompetencia.HasValue &&
                    m.DataCompetencia.Value.Year == ano &&
                    m.DataCompetencia.Value.Month == mes)
                .ToList();

            return caixas.Select(caixa => new CaixaResumoMensalDTO
            {
                CaixaId = caixa.Id,
                DescricaoCaixa = caixa.Descricao,
                ValorTotalMes = movimentacoes
                    .Where(m => m.CaixaId == caixa.Id)
                    .Sum(m => m.Valor)
            }).ToList();
        }

        public Caixa IncluirCaixa(CaixaSalvarModel model)
        {
            ValidarModelCaixa(model);
            ValidarLoja(model.LojaId);
            ValidarDuplicidadeCaixa(model, null);

            var entity = new Caixa
            {
                LojaId = model.LojaId,
                Descricao = model.Descricao!.Trim(),
                DataCadastro = DateTime.Now,
                Ativo = "V"
            };

            _context.Set<Caixa>().Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public Caixa AlterarCaixa(int id, CaixaSalvarModel model)
        {
            ValidarModelCaixa(model);

            var entity = ObterCaixaAtivo(id);

            ValidarLoja(model.LojaId);
            ValidarDuplicidadeCaixa(model, id);

            entity.LojaId = model.LojaId;
            entity.Descricao = model.Descricao!.Trim();

            _context.Set<Caixa>().Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public void DeletarCaixa(int id)
        {
            var entity = ObterCaixaAtivo(id);

            entity.Ativo = "F";

            _context.Set<Caixa>().Update(entity);
            _context.SaveChanges();
        }

        public CaixaMovimentacao IncluirMovimentacao(CaixaMovimentacaoSalvarModel model)
        {
            ValidarModelMovimentacao(model);

            _ = ObterCaixaAtivo(model.CaixaId);
            _ = ObterTipoValor(model.TipoValorCaixaId);

            var entity = new CaixaMovimentacao
            {
                CaixaId = model.CaixaId,
                TipoValorCaixaId = model.TipoValorCaixaId,
                Valor = model.Valor,
                DataCadastro = DateTime.Now,
                DataCompetencia = model.DataCompetencia,
                Descricao = model.Descricao,
                DataRealizacao = model.DataRealizacao,
                AnexoNome = model.AnexoNome,
                AnexoContentType = model.AnexoContentType,
                AnexoArquivo = model.AnexoArquivo,
                NomeFuncionario = model.NomeFuncionario,
                Ativo = "V"
            };

            _contextLocal.Set<CaixaMovimentacao>().Add(entity);
            _contextLocal.SaveChanges();

            return entity;
        }

        public CaixaMovimentacao AlterarMovimentacao(int id, CaixaMovimentacaoSalvarModel model)
        {
            ValidarModelMovimentacao(model);

            var entity = ObterMovimentacaoAtiva(id);

            _ = ObterCaixaAtivo(model.CaixaId);
            _ = ObterTipoValor(model.TipoValorCaixaId);

            entity.CaixaId = model.CaixaId;
            entity.TipoValorCaixaId = model.TipoValorCaixaId;
            entity.Valor = model.Valor;
            entity.DataCompetencia = model.DataCompetencia;
            entity.Descricao = model.Descricao;
            entity.DataRealizacao = model.DataRealizacao;
            entity.AnexoNome = model.AnexoNome;
            entity.AnexoContentType = model.AnexoContentType;
            entity.AnexoArquivo = model.AnexoArquivo;
            entity.NomeFuncionario = model.NomeFuncionario;

            _context.Set<CaixaMovimentacao>().Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public void DeletarMovimentacao(int id)
        {
            var entity = ObterMovimentacaoAtiva(id);

            entity.Ativo = "F";

            _context.Set<CaixaMovimentacao>().Update(entity);
            _context.SaveChanges();
        }

        private void ValidarData(int lojaId, DateTime data)
        {
            if (lojaId <= 0)
                throw new ArgumentException("LojaId é obrigatório.");

            if (data == DateTime.MinValue)
                throw new ArgumentException("Data inválida.");
        }

        private void ValidarModelCaixa(CaixaSalvarModel model)
        {
            if (model.LojaId <= 0)
                throw new ArgumentException("LojaId é obrigatório.");

            if (string.IsNullOrWhiteSpace(model.Descricao))
                throw new ArgumentException("Descrição é obrigatória.");
        }

        private void ValidarModelMovimentacao(CaixaMovimentacaoSalvarModel model)
        {
            if (model.CaixaId <= 0)
                throw new ArgumentException("CaixaId é obrigatório.");

            if (model.TipoValorCaixaId <= 0)
                throw new ArgumentException("TipoValorCaixaId é obrigatório.");

            if (model.Valor <= 0)
                throw new ArgumentException("Valor é obrigatório.");

            if (!model.DataCompetencia.HasValue)
                throw new ArgumentException("DataCompetencia é obrigatória.");
        }

        private void ValidarLoja(int lojaId)
        {
            var lojaExiste = _contextLocal.Lojas.Any(x => x.Id == lojaId);

            if (!lojaExiste)
                throw new ArgumentException("Loja não encontrada.");
        }

        private void ValidarDuplicidadeCaixa(CaixaSalvarModel model, int? idAtual)
        {
            var descricaoNormalizada = model.Descricao!.Trim();

            var query = _contextLocal.Set<Caixa>().Where(x =>
                x.LojaId == model.LojaId &&
                x.Descricao == descricaoNormalizada &&
                x.Ativo == "V");

            if (idAtual.HasValue)
                query = query.Where(x => x.Id != idAtual.Value);

            if (query.Any())
                throw new ArgumentException("Já existe caixa cadastrado para esta loja com esta descrição.");
        }

        private Caixa ObterCaixaAtivo(int id)
        {
            var entity = _contextLocal.Caixa.FirstOrDefault(x => x.Id == id && x.Ativo == "V");

            if (entity == null)
                throw new KeyNotFoundException("Caixa não encontrado.");

            return entity;
        }

        private CaixaMovimentacao ObterMovimentacaoAtiva(int id)
        {
            var entity = _contextLocal.CaixaMovimentacao.FirstOrDefault(x => x.Id == id && x.Ativo == "V");

            if (entity == null)
                throw new KeyNotFoundException("Movimentação não encontrada.");

            return entity;
        }

        private TipoValorCaixa ObterTipoValor(int id)
        {
            var entity = _contextLocal.TipoValorCaixa.FirstOrDefault(x => x.Id == id);

            if (entity == null)
                throw new ArgumentException("Tipo de valor inválido.");

            return entity;
        }

        public List<int> ListarDias(int ano, int mes)
        {
            if (mes < 1 || mes > 12)
                throw new ArgumentOutOfRangeException(nameof(mes), "O mês deve estar entre 1 e 12.");

            int totalDias = DateTime.DaysInMonth(ano, mes);
            List<int> dias = new List<int>();

            for (int i = 1; i <= totalDias; i++)
            {
                dias.Add(i);
            }

            return dias;
        }

        public List<TipoValorCaixa> ListarTiposValor()
        {
            return _contextLocal.TipoValorCaixa
                .OrderBy(t => t.Descricao)
                .ToList();
        }

        public TipoValorCaixa CriarTipoValor(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição é obrigatória.");

            var trimmed = descricao.Trim();

            var existente = _contextLocal.TipoValorCaixa
                .FirstOrDefault(t => t.Descricao == trimmed);

            if (existente != null)
                return existente;

            var entity = new TipoValorCaixa { Descricao = trimmed };
            _contextLocal.TipoValorCaixa.Add(entity);
            _contextLocal.SaveChanges();

            return entity;
        }
    }
}