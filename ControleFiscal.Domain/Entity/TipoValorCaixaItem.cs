#nullable disable

namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public partial class TipoValorCaixaItem
    {
        public int Id { get; set; }
        public int TipoValorCaixaId { get; set; }
        public string Descricao { get; set; }
        public string Ativo { get; set; } = "S";
        public DateTime? DataCadastro { get; set; } 
    }
}
