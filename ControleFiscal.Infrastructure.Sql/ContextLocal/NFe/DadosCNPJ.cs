using Newtonsoft.Json;

namespace ControleFiscal.Context.NFe
{
    public class DadosCnpj
    {
        [JsonProperty("nome")]
        public string? Nome { get; set; }

        [JsonProperty("fantasia")]
        public string? NomeFantasia { get; set; }

        [JsonProperty("cnpj")]
        public string? CNPJ { get; set; }

        [JsonProperty("situacao")]
        public string? Situacao { get; set; }

        [JsonProperty("logradouro")]
        public string? Logradouro { get; set; }

        [JsonProperty("numero")]
        public string? Numero { get; set; }

        [JsonProperty("bairro")]
        public string? Bairro { get; set; }

        [JsonProperty("municipio")]
        public string? Municipio { get; set; }

        [JsonProperty("uf")]
        public string? UF { get; set; }

        [JsonProperty("telefone")]
        public string? Telefone { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }
    }
}
