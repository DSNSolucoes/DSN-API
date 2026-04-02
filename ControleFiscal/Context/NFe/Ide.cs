using System.ComponentModel;
using System.Xml.Serialization;

namespace ControleFiscal.Context.NFe
{
    public class Ide
    {
        [XmlElement("cNF")]
        [Description("CNF: Código Numérico que compõe a Chave de Acesso.")]

        public string? cNF { get; set; }

        [XmlElement("cUF")]
        [Description("CNF: Código Municipio Emissao.")]
        public string? CodigoMunicipioEmissao { get; set; }

        [XmlElement("natOp")]
        [Description("NatOp: Descrição da Natureza da Operação.")]

        public string? NaturezaOperacao { get; set; }


        [Description("Mod: Modelo do Documento Fiscal.")]
        [XmlElement("mod")]
        public string? Modelo { get; set; }

        [Description("Serie: Série do Documento Fiscal.")]
        [XmlElement("serie")]
        public string? Serie { get; set; }

        [Description("NNF: Número do Documento Fiscal.")]
        [XmlElement("nNF")]
        public string? Numero { get; set; }

        [Description("DhEmi: Data e hora de emissão da NFe.")]
        [XmlElement("dhEmi")]
        public DateTime DataHoraEmissao { get; set; }

        [Description("DhSaiEnt: Data e hora de saída ou entrada da mercadoria/produto.")]
        [XmlElement("dhSaiEnt")]
        public DateTime? DataHoraEntradaSaida { get; set; }

        [Description("TpNF: Tipo de Operação (0- entrada; 1- saída).")]
        [XmlElement("tpNF")]
        public string? TipoOperacao { get; set; }

        [Description("IdDest: Identificador de local de destino da operação (1 - Interna; 2 - Interestadual; 3 - Exterior).")]
        [XmlElement("idDest")]
        public string? DestinoOperacao { get; set; }

        [Description("CMunFG: Código do Município de Ocorrência do Fato Gerador.")]
        [XmlElement("cMunFG")]
        public string? CodigoMunicipioFatoGerador { get; set; }

        [Description("TpImp: Formato de Impressão do DANFE.")]
        [XmlElement("tpImp")]
        public string? FormatoImpressao { get; set; }

        [Description("TpEmis: Forma de emissão da NF-e.")]
        [XmlElement("tpEmis")]
        public string? FormaEmissao { get; set; }

        [Description("CDV: Dígito Verificador da Chave de Acesso da NF-e.")]
        [XmlElement("cDV")]
        public string? DigitoVerificadorChavedeAcesso { get; set; }

        [Description("TpAmb: Identificação do Ambiente (1 - Produção; 2 - Homologação).")]
        [XmlElement("tpAmb")]
        public string? Ambiente { get; set; }

        [Description("FinNFe: Finalidade da emissão da NF-e.")]
        [XmlElement("finNFe")]
        public string? FinalidadeEmissao { get; set; }

        [Description("IndFinal: Indica operação com Consumidor final (0- Não; 1- Consumidor Final).")]
        [XmlElement("indFinal")]
        public string? EhConsumidorFinal { get; set; }

        [Description("IndPres: Indicador de presença do comprador no estabelecimento comercial no momento da operação.")]
        [XmlElement("indPres")]
        public string? indPres { get; set; }

        [Description("ProcEmi: Processo de emissão da NF-e.")]
        [XmlElement("procEmi")]
        public string? ProcessoEmissao { get; set; }

        [Description("VerProc: Versão do Processo de emissão da NF-e.")]
        [XmlElement("verProc")]
        public string? VersaoProcessoEmissao { get; set; }

        [Description("DhCont: Data e hora da entrada em contingência.")]
        [XmlElement("dhCont ")]
        public DateTime? DataHoraContingencia { get; set; }

        [Description("XJust: Justificativa da entrada em contingência.")]
        [XmlElement("XJust")]
        public string? XJust { get; set; }




    }
}