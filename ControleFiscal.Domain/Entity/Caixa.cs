 

namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public partial class Caixa
    {
        public string Id { get; set; } = string.Empty;
        public string LojaId { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string Ativo { get; set; } = "V";
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string SyncStatus { get; set; } = "PENDING";
    }
}