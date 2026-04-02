using System;
using System.Collections.Generic;

namespace ControleFiscal.Domain.DTO.Focus
{
    public partial class ProdutosDTO
    { 
        public int CdProduto { get; set; }
        public string? NmProduto { get; set; }
        public string? CodBarras { get; set; }
        public decimal? Precocusto { get; set; }
        public decimal? Margem { get; set; }
        public decimal? Precovenda { get; set; }
        public decimal? PdvPrecovenda { get; set; }
        public string? NomeLoja { get; set; }
        public string? Erro { get; set; }
    }
}