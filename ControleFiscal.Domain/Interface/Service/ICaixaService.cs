using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Domain.Model;
using ControleFiscal.Infrastructure.Sql.Entity;

namespace ControleFiscal.Services.Interfaces
{
    public interface ICaixaService
    {
        List<CaixaDTO> Listar(string lojaId, DateTime data);
        List<CaixaResumoMensalDTO> ListarResumoMensal(string lojaId, int ano, int mes);
        Caixa IncluirCaixa(CaixaSalvarModel model);
        Caixa AlterarCaixa(string id, CaixaSalvarModel model);
        void DeletarCaixa(string id);
        CaixaMovimentacao IncluirMovimentacao(CaixaMovimentacaoSalvarModel model);
        CaixaMovimentacao AlterarMovimentacao(string id, CaixaMovimentacaoSalvarModel model);
        void DeletarMovimentacao(string id);
        List<Caixa> ListarCaixas(string lojaId);
        List<int> ListarDias(int ano, int mes);
        List<TipoValorCaixa> ListarTiposValor();
        TipoValorCaixa CriarTipoValor(string descricao);
        TipoValorCaixa AlterarTipoValor(string id, string descricao);
        void DeletarTipoValor(string id);
    }
}
