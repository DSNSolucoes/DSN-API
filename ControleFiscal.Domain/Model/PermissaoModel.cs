namespace ControleFiscal.Domain.Model
{
    public class PermissaoSalvarModel
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
    }

    public class VincularPermissaoModel
    {
        public int PermissaoId { get; set; }
    }
}
