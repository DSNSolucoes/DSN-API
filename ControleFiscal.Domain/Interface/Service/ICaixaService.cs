using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Domain.Model;
using ControleFiscal.Infrastructure.Sql.Entity;

namespace ControleFiscal.Services.Interfaces
{
    public interface ICaixaService
    {
        List<CaixaDTO> Listar(int lojaId, DateTime data);
        List<CaixaResumoMensalDTO> ListarResumoMensal(int lojaId, int ano, int mes);
        Caixa IncluirCaixa(CaixaSalvarModel model);
        Caixa AlterarCaixa(int id, CaixaSalvarModel model);
        void DeletarCaixa(int id);

        CaixaMovimentacao IncluirMovimentacao(CaixaMovimentacaoSalvarModel model);
        CaixaMovimentacao AlterarMovimentacao(int id, CaixaMovimentacaoSalvarModel model);
        void DeletarMovimentacao(int id);

        List<int> ListarDias(int ano, int mes);
        List<TipoValorCaixa> ListarTiposValor();
        TipoValorCaixa CriarTipoValor(string descricao);
    }
}