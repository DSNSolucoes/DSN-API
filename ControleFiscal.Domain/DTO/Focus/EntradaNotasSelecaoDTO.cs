using System;
using System.Collections.Generic;

namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public partial class EntradaNotasSelecaoDTO
    {
        public string? CdNota { get; set; }
        public DateTime DtEntrada { get; set; }
        public string? Cnpj { get; set; }
        public string? NmFornecedor { get; set; }
        public string? NumDocumento { get; set; }
        public string? Serie { get; set; }
        public decimal ValorTotalNota { get; set; }

    }
}