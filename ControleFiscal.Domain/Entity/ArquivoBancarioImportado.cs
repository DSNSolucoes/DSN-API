namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class ArquivoBancarioImportado
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdContaBancaria { get; set; }
        public string NomeOriginal { get; set; } = string.Empty;
        public string Formato { get; set; } = string.Empty;
        public int? TamanhoBytes { get; set; }
        public string? HashArquivo { get; set; }
        public string Status { get; set; } = "RECEBIDO";
        public int TotalLidos { get; set; } = 0;
        public int TotalImportados { get; set; } = 0;
        public int TotalDuplicados { get; set; } = 0;
        public int TotalErros { get; set; } = 0;
        public string? LogErro { get; set; }
        public int? IdResponsavel { get; set; }
        public DateTime DataUpload { get; set; } = DateTime.Now;
        public DateTime? DataProcessamento { get; set; }

        public Empresa? Empresa { get; set; }
        public ContaBancaria? ContaBancaria { get; set; }
        public ICollection<ItemImportacaoBancaria> Itens { get; set; } = new List<ItemImportacaoBancaria>();
    }
}
