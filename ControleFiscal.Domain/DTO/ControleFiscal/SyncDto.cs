namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public class SyncResultDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public int RegistrosEnviados { get; set; }
        public int RegistrosRecebidos { get; set; }
        public int Conflitos { get; set; }
        public DateTime? UltimoSync { get; set; }
        public List<string> Erros { get; set; } = new();
    }

    public class SyncStatusDto
    {
        public bool EmSincronizacao { get; set; }
        public DateTime? UltimoSync { get; set; }
        public string? UrlNuvem { get; set; }
        public int PendenteLocal { get; set; }
        public bool ConectividadeNuvem { get; set; }
    }

    public class SyncRegistroDto
    {
        public string Tabela { get; set; } = string.Empty;
        public string RegistroId { get; set; } = string.Empty;
        public string Operacao { get; set; } = string.Empty;   // INSERT | UPDATE | DELETE
        public string Payload { get; set; } = string.Empty;    // JSON serializado
        public DateTime AtualizadoEm { get; set; }
    }

    public class SyncPacoteDto
    {
        public string OrigemId { get; set; } = string.Empty;    // UUID da instalação local
        public DateTime EnviadoEm { get; set; } = DateTime.UtcNow;
        public List<SyncRegistroDto> Registros { get; set; } = new();
    }

    public class SyncAlteracoesNuvemDto
    {
        public List<SyncRegistroDto> Registros { get; set; } = new();
        public DateTime ConsultadoEm { get; set; }
    }

    public class SyncConfigurarModel
    {
        public string UrlNuvem { get; set; } = string.Empty;
    }
}
