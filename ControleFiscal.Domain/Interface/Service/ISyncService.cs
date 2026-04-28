using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Infrastructure.Sql.Entity;

namespace ControleFiscal.Domain.Interface.Service
{
    public interface ISyncService
    {
        /// <summary>
        /// Executa o ciclo completo de sincronização: envia PENDING locais para nuvem e
        /// puxa alterações da nuvem desde o último sync. Retorna resultado com estatísticas.
        /// </summary>
        Task<SyncResultDto> SincronizarAsync(CancellationToken ct = default);

        /// <summary>Retorna o status atual da sincronização (última data, pendentes, etc.).</summary>
        Task<SyncStatusDto> ObterStatusAsync();

        /// <summary>Define ou atualiza a URL base da API na nuvem.</summary>
        Task ConfigurarUrlNuvemAsync(string url);

        /// <summary>Retorna logs de sincronização paginados.</summary>
        IEnumerable<SyncLog> ObterLogs(int pagina = 0, int tamanho = 50);
    }
}
