 

namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public partial class Caixa
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string Ativo { get; set; } = "V";
        public short Ordem { get; set; }
    }
}