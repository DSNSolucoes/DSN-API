#nullable disable

namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public partial class TipoValorCaixa
    {
        public string Id { get; set; } = string.Empty;
        public string Descricao { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string SyncStatus { get; set; } = "PENDING";
    }
}