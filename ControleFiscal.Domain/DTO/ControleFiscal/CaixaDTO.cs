namespace ControleFiscal.Domain.DTO.ControleFiscal
{

    public class CaixaDTO
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public string NomeLoja { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string Ativo { get; set; }

        // competência usada na montagem do retorno
        public short AnoCompetencia { get; set; }
        public short MesCompetencia { get; set; }

        public List<CaixaMovimentacoesDTO> Valores { get; set; } = new();
    }

}