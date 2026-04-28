namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public class CaixaDTO
    {
        public string Id { get; set; } = string.Empty;
        public string LojaId { get; set; } = string.Empty;
        public string NomeLoja { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime? DataCadastro { get; set; }
        public string Ativo { get; set; } = "V";
        public short AnoCompetencia { get; set; }
        public short MesCompetencia { get; set; }
        public List<CaixaMovimentacoesDTO> Valores { get; set; } = new();
    }
}
