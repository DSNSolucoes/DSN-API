using ControleFiscal.Domain.DTO.Bancario;
using ControleFiscal.Domain.Model.Bancario;
using ControleFiscal.Infrastructure.Sql.Entity;

namespace ControleFiscal.Services.Interfaces
{
    public interface IBancoService
    {
        List<BancoDTO> Listar();
        BancoDTO Obter(int id);
        Banco Criar(BancoSalvarModel model);
        Banco Alterar(int id, BancoSalvarModel model);
        void Deletar(int id);
    }

    public interface IContaBancariaService
    {
        List<ContaBancariaDTO> Listar(int empresaId);
        ContaBancariaDTO Obter(int empresaId, int id);
        ContaBancaria Criar(ContaBancariaSalvarModel model);
        ContaBancaria Alterar(int id, ContaBancariaSalvarModel model);
        void Inativar(int id);
        SaldoContaDTO ObterSaldo(int empresaId, int id);
        SaldoEmpresaDTO ObterSaldoEmpresa(int empresaId);
    }

    public interface ICategoriaFinanceiraService
    {
        List<CategoriaFinanceiraDTO> Listar(int empresaId);
        CategoriaFinanceiraDTO Obter(int empresaId, int id);
        CategoriaFinanceira Criar(CategoriaFinanceiraSalvarModel model);
        CategoriaFinanceira Alterar(int id, CategoriaFinanceiraSalvarModel model);
        void Inativar(int id);
    }

    public interface ILancamentoBancarioService
    {
        List<LancamentoBancarioDTO> Listar(int empresaId, int contaId, DateTime dataInicio, DateTime dataFim);
        LancamentoBancarioDTO Obter(int empresaId, int id);
        LancamentoBancario Criar(LancamentoBancarioSalvarModel model);
        LancamentoBancario Alterar(int id, LancamentoBancarioSalvarModel model);
        void Cancelar(int id, int idResponsavel);
    }

    public interface IConciliacaoBancariaService
    {
        List<LancamentoBancarioDTO> ListarPendentes(int empresaId, int? contaId);
        List<ConciliacaoBancariaDTO> SugerirConciliacao(int empresaId, int contaId);
        ConciliacaoBancaria Conciliar(ConciliarModel model);
        void Desconciliar(DesconciliarModel model);
    }

    public interface IFechamentoBancarioService
    {
        List<FechamentoBancarioDTO> Listar(int empresaId);
        FechamentoBancarioDTO Obter(int empresaId, int contaId, int ano, int mes);
        FechamentoBancarioMensal Fechar(FecharMesModel model);
        void Reabrir(int id, int idResponsavel);
    }

    public interface IRegraClassificacaoService
    {
        List<RegraClassificacaoDTO> Listar(int empresaId);
        RegraClassificacaoBancaria Criar(RegraClassificacaoSalvarModel model);
        RegraClassificacaoBancaria Alterar(int id, RegraClassificacaoSalvarModel model);
        void Deletar(int id);
    }

    public interface IDashboardBancarioService
    {
        DashboardBancarioDTO ObterDashboard(int empresaId, int ano, int mes);
    }
}
