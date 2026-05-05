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
        public List<CaixaDTO> Listar(int IdEmpresa, DateTime data)
        {
            ValidarData(IdEmpresa, data);

            var nomeLoja = _contextLocal.Empresas
                .Where(x => x.Id == IdEmpresa)
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

            var tiposValorCaxiaItem = _contextLocal.TipoValorCaixaItem
                .OrderBy(t => t.Descricao)
                .Select(t => new TipoValor
                {
                    Id = t.Id,
                    Descricao = t.Descricao
                })
                .ToList();

            var caixas = _contextLocal.Caixa
                .Where(c => c.IdEmpresa == IdEmpresa && c.Ativo == "V")
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
                            DataCompetencia = m.DataCompetencia ?? data,
                            DataRealizacao = m.DataRealizacao ?? data,
                            AnoCompetencia = data.Year,
                            MesCompetencia = data.Month,
                            TipoValorCaixaId = m.TipoValorCaixaId,
                            NomeFuncionario = m.NomeFuncionario ?? string.Empty,
                            TipoValorItemId = m.TipoValorCaixaItemId,
                            TipoValorItemDescricao = m.TipoValorCaixaItemId.HasValue
                                ? tiposValorCaxiaItem.FirstOrDefault(x => x.Id == m.TipoValorCaixaItemId)?.Descricao
                                : null,
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
                    IdEmpresa = caixa.IdEmpresa,
                    NomeLoja = nomeLoja,
                    Descricao = caixa.Descricao,
                    Ordem = caixa.Ordem,
                    AnoCompetencia = (short)data.Year,
                    MesCompetencia = (short)data.Month,
                    Valores = valores
                };
            }).OrderBy(x => x.Ordem).ToList();

            return retorno;
        }

        public List<CaixaResumoMensalDTO> ListarResumoMensal(int IdEmpresa, int ano, int mes)
        {
            if (IdEmpresa <= 0)
                throw new ArgumentException("IdEmpresa é obrigatório.");

            if (ano <= 0)
                throw new ArgumentException("Ano inválido.");

            if (mes < 1 || mes > 12)
                throw new ArgumentException("Mês inválido.");

            var caixas = _contextLocal.Caixa
                .Where(c => c.IdEmpresa == IdEmpresa && c.Ativo == "V")
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
            ValidarLoja(model.IdEmpresa);
            ValidarDuplicidadeCaixa(model, null);

            var entity = new Caixa
            {
                IdEmpresa = model.IdEmpresa,
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

            ValidarLoja(model.IdEmpresa);
            ValidarDuplicidadeCaixa(model, id);

            entity.IdEmpresa = model.IdEmpresa;
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
                TipoValorCaixaItemId = model.TipoValorCaixaItemId,
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
            entity.TipoValorCaixaItemId = model.TipoValorCaixaItemId;

            _contextLocal.Set<CaixaMovimentacao>().Update(entity);
            _contextLocal.SaveChanges();

            return entity;
        }

        public void DeletarMovimentacao(int id, string nomeUsuario)
        {
            var entity = ObterMovimentacaoAtiva(id);

            entity.Ativo = "F";
            entity.NomeUsuarioExclusao = nomeUsuario?.Trim();
            entity.DataExclusao = DateTime.Now;

            _contextLocal.CaixaMovimentacao.Update(entity);
            _contextLocal.SaveChanges();
        }

        private void ValidarData(int IdEmpresa, DateTime data)
        {
            if (IdEmpresa <= 0)
                throw new ArgumentException("IdEmpresa é obrigatório.");

            if (data == DateTime.MinValue)
                throw new ArgumentException("Data inválida.");
        }

        private void ValidarModelCaixa(CaixaSalvarModel model)
        {
            if (model.IdEmpresa <= 0)
                throw new ArgumentException("IdEmpresa é obrigatório.");

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

        private void ValidarLoja(int IdEmpresa)
        {
            var lojaExiste = _contextLocal.Empresas.Any(x => x.Id == IdEmpresa);

            if (!lojaExiste)
                throw new ArgumentException("Loja não encontrada.");
        }

        private void ValidarDuplicidadeCaixa(CaixaSalvarModel model, int? idAtual)
        {
            var descricaoNormalizada = model.Descricao!.Trim();

            var query = _contextLocal.Set<Caixa>().Where(x =>
                x.IdEmpresa == model.IdEmpresa &&
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

        public List<TipoValorCaixaItem> ListarItensDoTipoValor(int tipoValorId)
        {
            return _contextLocal.TipoValorCaixaItem
                .Where(i => i.TipoValorCaixaId == tipoValorId && i.Ativo == "V")
                .OrderBy(i => i.Descricao)
                .ToList();
        }

        public TipoValorCaixaItem CriarItemTipoValor(int tipoValorId, string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição é obrigatória.");

            var tipoExiste = _contextLocal.TipoValorCaixa.Any(t => t.Id == tipoValorId);
            if (!tipoExiste)
                throw new ArgumentException("Tipo de valor não encontrado.");

            var trimmed = descricao.Trim();

            var existente = _contextLocal.TipoValorCaixaItem
                .FirstOrDefault(i => i.TipoValorCaixaId == tipoValorId && i.Descricao == trimmed && i.Ativo == "V");

            if (existente != null)
                return existente;

            var entity = new TipoValorCaixaItem
            {
                TipoValorCaixaId = tipoValorId,
                Descricao = trimmed,
                Ativo = "S",
                DataCadastro = DateTime.Now
            };

            _contextLocal.TipoValorCaixaItem.Add(entity);
            _contextLocal.SaveChanges();

            return entity;
        }

        public void DeletarItemTipoValor(int itemId)
        {
            var entity = _contextLocal.TipoValorCaixaItem.FirstOrDefault(i => i.Id == itemId && i.Ativo == "V");

            if (entity == null)
                throw new KeyNotFoundException("Item não encontrado.");

            entity.Ativo = "N";
            _contextLocal.TipoValorCaixaItem.Update(entity);
            _contextLocal.SaveChanges();
        }
    }
}