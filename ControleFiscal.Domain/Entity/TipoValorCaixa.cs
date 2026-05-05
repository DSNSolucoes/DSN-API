#nullable disable

namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public partial class TipoValorCaixa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<TipoValorCaixaItem> Itens { get; set; } = new List<TipoValorCaixaItem>();
    }
}