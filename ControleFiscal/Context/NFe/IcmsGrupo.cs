using System.Xml.Serialization;

namespace ControleFiscal.Context.NFe
{
    public class IcmsGrupo
    {      
        [XmlElement("ICMS00")]
        public ICMS00? TribICMS00 { get; set; }

        [XmlElement("ICMS10")]
        public ICMS10? TribICMS10 { get; set; }

        [XmlElement("ICMS20")]
        public ICMS20? TribICMS20 { get; set; }

        [XmlElement("ICMS30")]
        public ICMS30? TribICMS30 { get; set; }

        [XmlElement("ICMS40")]
        public ICMS40? TribICMS40 { get; set; }

        [XmlElement("ICMS51")]
        public ICMS51? TribICMS51 { get; set; }

        [XmlElement("ICMS60")]
        public ICMS60? TribICMS60 { get; set; }


        [XmlElement("ICMS70")]
        public ICMS70? TribICMS70 { get; set; }

        [XmlElement("ICMS90")]
        public ICMS90? TribICMS90 { get; set; }

        [XmlElement("ICMSPart")]
        public ICMSPart? TribICMSPart { get; set; }

        [XmlElement("ICMSSN101")]
        public ICMSSN101? TribICMSSN101 { get; set; }

        [XmlElement("ICMSSN102")]
        public ICMSSN102? TribICMSSN102 { get; set; }

        [XmlElement("ICMSSN201")]
        public ICMSSN201? TribICMSSN201 { get; set; }

        [XmlElement("ICMSSN202")]
        public ICMSSN202? TribICMSSN202 { get; set; }

        [XmlElement("ICMSSN500")]
        public ICMSSN500? TribICMSSN500 { get; set; }

        [XmlElement("ICMSSN900")]
        public ICMSSN900? TribICMSSN900 { get; set; }


        public class ICMS00
        {
            public string? Orig { get; set; }
            public string? CST { get; set; }
            public string? ModBC { get; set; }
            public decimal VBC { get; set; }
            public decimal PICMS { get; set; }
            public decimal VICMS { get; set; }
        }

        public class ICMS10
        {
            public string? Orig { get; set; }
            public string? CST { get; set; }
            public string? ModBC { get; set; }
            public decimal VBC { get; set; }
            public decimal PICMS { get; set; }
            public decimal VICMS { get; set; }
            public string ModBCST { get; set; }
            public decimal PMVAST { get; set; }
            public decimal PRedBCST { get; set; }
            public decimal VBCST { get; set; }
            public decimal PICMSST { get; set; }
            public decimal VICMSST { get; set; }
        }

        public class ICMS20
        {
            public string? Orig { get; set; }
            public string? CST { get; set; }
            public string? ModBC { get; set; }
            public decimal PRedBC { get; set; }
            public decimal VBC { get; set; }
            public decimal PICMS { get; set; }
            public decimal VICMS { get; set; }
        }

        public class ICMS30
        {
            public string? Orig { get; set; }
            public string? CST { get; set; }
            public string? ModBCST { get; set; }
            public decimal PMVAST { get; set; }
            public decimal PRedBCST { get; set; }
            public decimal VBCST { get; set; }
            public decimal PICMSST { get; set; }
            public decimal VICMSST { get; set; }
        }

        public class ICMS40
        {
            public string? Orig { get; set; }
            public string? CST { get; set; }
        }

        public class ICMS51
        {
            public string? Orig { get; set; }
            public string? CST { get; set; }
            public string? ModBC { get; set; }
            public decimal PRedBC { get; set; }
            public decimal VBC { get; set; }
            public decimal PICMS { get; set; }
            public decimal VICMS { get; set; }
            public decimal VICMSOp { get; set; }
            public decimal PDif { get; set; }
            public decimal VICMSDif { get; set; }
        }

        public class ICMS60
        {
            public string? Orig { get; set; }
            public string? CST { get; set; }
            public decimal VBCSTRet { get; set; }
            public decimal VICMSSTRet { get; set; }
        }

        public class ICMS70
        {
            public string? Orig { get; set; }
            public string CST { get; set; }
            public string? ModBC { get; set; }
            public decimal PRedBC { get; set; }
            public decimal VBC { get; set; }
            public decimal PICMS { get; set; }
            public decimal VICMS { get; set; }
            public string? ModBCST { get; set; }
            public decimal PMVAST { get; set; }
            public decimal PRedBCST { get; set; }
            public decimal VBCST { get; set; }
            public decimal PICMSST { get; set; }
            public decimal VICMSST { get; set; }
        }

        public class ICMS90
        {
            public string? Orig { get; set; }
            public string? CST { get; set; }
            public string? ModBC { get; set; }
            public decimal VBC { get; set; }
            public decimal PICMS { get; set; }
            public decimal VICMS { get; set; }
            public string? ModBCST { get; set; }
            public decimal PMVAST { get; set; }
            public decimal PRedBCST { get; set; }
            public decimal VBCST { get; set; }
            public decimal PICMSST { get; set; }
            public decimal VICMSST { get; set; }
        }

        public class ICMSPart
        {
            public string? Orig { get; set; }
            public string? CST { get; set; }
            public string? ModBC { get; set; }
            public decimal VBC { get; set; }
            public decimal PICMS { get; set; }
            public decimal VICMS { get; set; }
            public string? ModBCST { get; set; }
            public decimal PMVAST { get; set; }
            public decimal PRedBCST { get; set; }
            public decimal VBCST { get; set; }
            public decimal PICMSST { get; set; }
            public decimal VICMSST { get; set; }
            public decimal PBCOp { get; set; }
            public decimal UFST { get; set; }
        }

        public class ICMSSN101
        {
            public string? Orig { get; set; }
            public string? CSOSN { get; set; }
            public decimal PCredSN { get; set; }
            public decimal VCredICMSSN { get; set; }
        }

        public class ICMSSN102
        {
            public string? Orig { get; set; }
            public string? CSOSN { get; set; }
        }

        public class ICMSSN201
        {
            public string? Orig { get; set; }
            public string? CSOSN { get; set; }
            public string ModBCST { get; set; }
            public decimal PMVAST { get; set; }
            public decimal PRedBCST { get; set; }
            public decimal VBCST { get; set; }
            public decimal PICMSST { get; set; }
            public decimal VICMSST { get; set; }
            public decimal PCredSN { get; set; }
            public decimal VCredICMSSN { get; set; }
        }

        public class ICMSSN202
        {
            public string? Orig { get; set; }
            public string? CSOSN { get; set; }
            public string? ModBCST { get; set; }
            public decimal PMVAST { get; set; }
            public decimal PRedBCST { get; set; }
            public decimal VBCST { get; set; }
            public decimal PICMSST { get; set; }
            public decimal VICMSST { get; set; }
        }

        public class ICMSSN500
        {
            public string? Orig { get; set; }
            public string? CSOSN { get; set; }
            public decimal VBCSTRet { get; set; }
            public decimal VICMSSTRet { get; set; }
        }

        public class ICMSSN900
        {
            public string? Orig { get; set; }
            public string? CSOSN { get; set; }
            public string? ModBC { get; set; }
            public decimal VBC { get; set; }
            public decimal PICMS { get; set; }
            public decimal VICMS { get; set; }
            public string? ModBCST { get; set; }
            public decimal PMVAST { get; set; }
            public decimal PRedBCST { get; set; }
            public decimal VBCST { get; set; }
            public decimal PICMSST { get; set; }
            public decimal VICMSST { get; set; }
            public decimal PCredSN { get; set; }
            public decimal VCredICMSSN { get; set; }
        }

    }

}
