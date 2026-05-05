using ControleFiscal.Domain.DTO.ContasPagar;
using ControleFiscal.Domain.Model.ContasPagar;
using ControleFiscal.Infrastructure.Sql.Entity;

namespace ControleFiscal.Services.Interfaces
{
    public interface IFornecedorCPService
    {
        List<FornecedorDTO> Listar(int empresaId);
        FornecedorDTO Obter(int empresaId, int cdFornecedor);
        Fornecedor Criar(FornecedorSalvarModel model);
        Fornecedor Alterar(int cdFornecedor, FornecedorSalvarModel model);
        void Inativar(int cdFornecedor);
    }

    public interface ICategoriaContaPagarService
    {
        List<CategoriaContaPagarDTO> Listar(int empresaId);
        CategoriaContaPagarDTO Obter(int empresaId, int id);
        CategoriaContaPagar Criar(CategoriaContaPagarSalvarModel model);
        CategoriaContaPagar Alterar(int id, CategoriaContaPagarSalvarModel model);
        void Inativar(int id);
    }

    public interface ICentroCustoCPService
    {
        List<CentroCustoCPDTO> Listar(int empresaId);
        CentroCustoCP Criar(CentroCustoCPSalvarModel model);
        CentroCustoCP Alterar(int id, CentroCustoCPSalvarModel model);
        void Inativar(int id);
    }

    public interface IContaPagarService
    {
        List<ContaPagarDTO> Listar(FiltroContasPagarModel filtro);
        ContaPagarDTO Obter(int empresaId, int id);
        ContaPagar Criar(ContaPagarSalvarModel model);
        ContaPagar Alterar(int id, ContaPagarSalvarModel model);
        List<ContaPagar> GerarParcelas(GerarParcelasModel model);
        void Cancelar(int id, CancelarContaModel model);
        void Reabrir(int id, ReobrirContaModel model);
        void AtualizarVencidas(int empresaId);
    }

    public interface IPagamentoContaPagarService
    {
        List<PagamentoContaPagarDTO> ListarPorConta(int empresaId, int contaPagarId);
        PagamentoContaPagar Registrar(int contaPagarId, RegistrarPagamentoModel model);
        void Estornar(int pagamentoId, EstornarPagamentoModel model);
    }

    public interface IContaPagarRecorrenteService
    {
        List<ContaPagarRecorrenteDTO> Listar(int empresaId);
        ContaPagarRecorrenteDTO Obter(int empresaId, int id);
        ContaPagarRecorrente Criar(ContaPagarRecorrenteSalvarModel model);
        ContaPagarRecorrente Alterar(int id, ContaPagarRecorrenteSalvarModel model);
        void Cancelar(int id, int? idResponsavel);
        List<ContaPagar> GerarContas(int id, GerarContasRecorrentesModel model);
    }

    public interface IDashboardContasPagarService
    {
        DashboardContasPagarDTO Obter(int empresaId);
    }
}
