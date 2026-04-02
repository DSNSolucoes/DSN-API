using ControleFiscal.Context.NFe;
using ControleFiscal.Controllers;
using System.Xml.Linq;

namespace ControleFiscal.Services.NFeService
{
    public class NFeXmlReader
    {
        public static NFe ReadNFeFromXml(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            XNamespace ns = "http://www.portalfiscal.inf.br/nfe";

            var dd = new NFeSerialization().GetObjectFromFile<string>(filePath);
            return new NFe();

            //    if (doc != null)
            //{
            //    NFe teste = new NFe();
            //    var tt = doc.Descendants("infNFe"); 
            //    var rr = tt.Descendants("ide");

            //    teste.Ide = doc.Descendants(ns + "ide").Select(i => new Ide
            //    {
            //        CNF = i.Element("cNF")?.Value,
            //        NatOp = i.Element("natOp")?.Value,
            //        Mod = i.Element("mod")?.Value,
            //        Serie = i.Element("serie")?.Value,
            //        NNF = i.Element("nNF")?.Value,
            //        DhEmi = DateTime.Parse(i.Element("dhEmi")?.Value),
            //        DhSaiEnt = i.Element("dhSaiEnt") != null ? DateTime.Parse(i.Element("dhSaiEnt").Value) : (DateTime?)null,
            //        TpNF = i.Element("tpNF")?.Value,
            //        IdDest = i.Element("idDest")?.Value,
            //        CMunFG = i.Element("cMunFG")?.Value,
            //        TpImp = i.Element("tpImp")?.Value,
            //        TpEmis = i.Element("tpEmis")?.Value,
            //        CDV = i.Element("cDV")?.Value,
            //        TpAmb = i.Element("tpAmb")?.Value,
            //        FinNFe = i.Element("finNFe")?.Value,
            //        IndFinal = i.Element("indFinal")?.Value,
            //        IndPres = i.Element("indPres")?.Value,
            //        ProcEmi = i.Element("procEmi")?.Value,
            //        VerProc = i.Element("verProc")?.Value,
            //    }).FirstOrDefault();

            //    return new NFe
            //    {
            //        Ide = doc.Descendants("ide").Select(i => new Ide
            //        {
            //            CNF = i.Element("cNF")?.Value,
            //            NatOp = i.Element("natOp")?.Value,
            //            Mod = i.Element("mod")?.Value,
            //            Serie = i.Element("serie")?.Value,
            //            NNF = i.Element("nNF")?.Value,
            //            DhEmi = DateTime.Parse(i.Element("dhEmi")?.Value),
            //            DhSaiEnt = i.Element("dhSaiEnt") != null ? DateTime.Parse(i.Element("dhSaiEnt").Value) : (DateTime?)null,
            //            TpNF = i.Element("tpNF")?.Value,
            //            IdDest = i.Element("idDest")?.Value,
            //            CMunFG = i.Element("cMunFG")?.Value,
            //            TpImp = i.Element("tpImp")?.Value,
            //            TpEmis = i.Element("tpEmis")?.Value,
            //            CDV = i.Element("cDV")?.Value,
            //            TpAmb = i.Element("tpAmb")?.Value,
            //            FinNFe = i.Element("finNFe")?.Value,
            //            IndFinal = i.Element("indFinal")?.Value,
            //            IndPres = i.Element("indPres")?.Value,
            //            ProcEmi = i.Element("procEmi")?.Value,
            //            VerProc = i.Element("verProc")?.Value,
            //        }).FirstOrDefault(),

            //        Emit = doc.Descendants( "emit").Select(e => new Emitente
            //        {
            //            CNPJ = e.Element( "CNPJ")?.Value,
            //            CPF = e.Element( "CPF")?.Value,
            //            XNome = e.Element( "xNome")?.Value,
            //            XFant = e.Element( "xFant")?.Value,
            //            IE = e.Element( "IE")?.Value,
            //            IEST = e.Element( "IEST")?.Value,
            //            IM = e.Element( "IM")?.Value,
            //            CNAE = e.Element( "CNAE")?.Value,
            //            CRT = e.Element( "CRT")?.Value,
            //            Endereco = new Endereco
            //            {
            //                XLgr = e.Element( "enderEmit")?.Element( "xLgr")?.Value,
            //                Nro = e.Element( "enderEmit")?.Element( "nro")?.Value,
            //                XCpl = e.Element( "enderEmit")?.Element( "xCpl")?.Value,
            //                XBairro = e.Element( "enderEmit")?.Element( "xBairro")?.Value,
            //                CMun = e.Element( "enderEmit")?.Element( "cMun")?.Value,
            //                XMun = e.Element( "enderEmit")?.Element( "xMun")?.Value,
            //                UF = e.Element( "enderEmit")?.Element( "UF")?.Value,
            //                CEP = e.Element( "enderEmit")?.Element( "CEP")?.Value,
            //                CPais = e.Element( "enderEmit")?.Element( "cPais")?.Value,
            //                XPais = e.Element( "enderEmit")?.Element( "xPais")?.Value,
            //                Fone = e.Element( "enderEmit")?.Element( "fone")?.Value
            //            }
            //        }).FirstOrDefault(),


            //        Dest = doc.Descendants( "dest").Select(d => new Destinatario
            //        {
            //            CNPJ = d.Element( "CNPJ")?.Value,
            //            CPF = d.Element( "CPF")?.Value,
            //            XNome = d.Element( "xNome")?.Value,
            //            IndIEDest = d.Element( "indIEDest")?.Value,
            //            IE = d.Element( "IE")?.Value,
            //            ISUF = d.Element( "ISUF")?.Value,
            //            Email = d.Element( "email")?.Value,
            //            Endereco = new Endereco
            //            {
            //                XLgr = d.Element( "enderDest")?.Element( "xLgr")?.Value,
            //                Nro = d.Element( "enderDest")?.Element( "nro")?.Value,
            //                XCpl = d.Element( "enderDest")?.Element( "xCpl")?.Value,
            //                XBairro = d.Element( "enderDest")?.Element( "xBairro")?.Value,
            //                CMun = d.Element( "enderDest")?.Element( "cMun")?.Value,
            //                XMun = d.Element( "enderDest")?.Element( "xMun")?.Value,
            //                UF = d.Element( "enderDest")?.Element( "UF")?.Value,
            //                CEP = d.Element( "enderDest")?.Element( "CEP")?.Value,
            //                CPais = d.Element( "enderDest")?.Element( "cPais")?.Value,
            //                XPais = d.Element( "enderDest")?.Element( "xPais")?.Value,
            //                Fone = d.Element( "enderDest")?.Element( "fone")?.Value
            //            }
            //        }).FirstOrDefault(),

            //        Produtos = doc.Descendants( "det").Select(d => new Produto
            //        {
            //            CProd = d.Element( "prod")?.Element( "cProd")?.Value,
            //            CEAN = d.Element( "prod")?.Element( "cEAN")?.Value,
            //            XProd = d.Element( "prod")?.Element( "xProd")?.Value,
            //            NCM = d.Element( "prod")?.Element( "NCM")?.Value,
            //            CEST = d.Element( "prod")?.Element( "CEST")?.Value,
            //            CFOP = d.Element( "prod")?.Element( "CFOP")?.Value,
            //            UCom = d.Element( "prod")?.Element( "uCom")?.Value,
            //            QCom = decimal.Parse(d.Element( "prod")?.Element( "qCom")?.Value, CultureInfo.InvariantCulture),
            //            VUnCom = decimal.Parse(d.Element( "prod")?.Element( "vUnCom")?.Value, CultureInfo.InvariantCulture),
            //            VProd = decimal.Parse(d.Element( "prod")?.Element( "vProd")?.Value, CultureInfo.InvariantCulture),
            //            CEANTrib = d.Element( "prod")?.Element( "cEANTrib")?.Value,
            //            UTrib = d.Element( "prod")?.Element( "uTrib")?.Value,
            //            QTrib = decimal.Parse(d.Element( "prod")?.Element( "qTrib")?.Value, CultureInfo.InvariantCulture),
            //            VUnTrib = decimal.Parse(d.Element( "prod")?.Element( "vUnTrib")?.Value, CultureInfo.InvariantCulture),
            //            VFrete = d.Element( "prod")?.Element( "vFrete") != null ? decimal.Parse(d.Element( "prod")?.Element( "vFrete").Value, CultureInfo.InvariantCulture) : (decimal?)null,
            //            VSeg = d.Element( "prod")?.Element( "vSeg") != null ? decimal.Parse(d.Element( "prod")?.Element( "vSeg").Value, CultureInfo.InvariantCulture) : (decimal?)null,
            //            VDesc = d.Element( "prod")?.Element( "vDesc") != null ? decimal.Parse(d.Element( "prod")?.Element( "vDesc").Value, CultureInfo.InvariantCulture) : (decimal?)null,
            //            VIPI = d.Element( "prod")?.Element( "vIPI") != null ? decimal.Parse(d.Element( "prod")?.Element( "vIPI").Value, CultureInfo.InvariantCulture) : (decimal?)null,
            //            VOutro = d.Element( "prod")?.Element( "vOutro") != null ? decimal.Parse(d.Element( "prod")?.Element( "vOutro").Value, CultureInfo.InvariantCulture) : (decimal?)null,
            //            IndTot = d.Element( "prod")?.Element( "indTot")?.Value,
            //            // Impostos
            //            Icms = d.Element( "imposto")?.Element( "ICMS")?.Elements().Select(icms => new Icms
            //            {
            //                // Exemplo genérico para ICMS00. Você deve ajustar conforme o CST específico.
            //                Orig = icms.Element( "ICMS00")?.Element( "orig")?.Value,
            //                CST = icms.Element( "ICMS00")?.Element( "CST")?.Value,
            //                ModBC = icms.Element( "ICMS00")?.Element( "modBC")?.Value,
            //                VBC = decimal.Parse(icms.Element( "ICMS00")?.Element( "vBC")?.Value ?? "0", CultureInfo.InvariantCulture),
            //                PICMS = decimal.Parse(icms.Element( "ICMS00")?.Element( "pICMS")?.Value ?? "0", CultureInfo.InvariantCulture),
            //                VICMS = decimal.Parse(icms.Element( "ICMS00")?.Element( "vICMS")?.Value ?? "0", CultureInfo.InvariantCulture),
            //            }).FirstOrDefault(),

            //            Ipi = d.Element( "imposto")?.Element( "IPI")?.Elements().Select(ipi => new Ipi
            //            {
            //                CNPJProd = ipi.Element( "CNPJProd")?.Value,
            //                CSelo = ipi.Element( "cSelo")?.Value,
            //                QSelo = ipi.Element( "qSelo") != null ? int.Parse(ipi.Element( "qSelo").Value) : (int?)null,
            //                CEnq = ipi.Element( "cEnq")?.Value,
            //                // Atribuição condicional para CST, baseada na presença de IPITrib ou IPINT
            //                CST = ipi.Elements( "IPITrib").Any() ? ipi.Element( "IPITrib")?.Element( "CST")?.Value :
            //                 ipi.Elements( "IPINT").Any() ? ipi.Element( "IPINT")?.Element( "CST")?.Value : null,
            //                VBC = ipi.Element( "IPITrib") != null ? decimal.Parse(ipi.Element( "IPITrib")?.Element( "vBC")?.Value ?? "0", CultureInfo.InvariantCulture) : (decimal?)null,
            //                PIPI = ipi.Element( "IPITrib") != null ? decimal.Parse(ipi.Element( "IPITrib")?.Element( "pIPI")?.Value ?? "0", CultureInfo.InvariantCulture) : (decimal?)null,
            //                VIPI = ipi.Element( "IPITrib") != null ? decimal.Parse(ipi.Element( "IPITrib")?.Element( "vIPI")?.Value ?? "0", CultureInfo.InvariantCulture) : (decimal?)null,
            //            }).FirstOrDefault(),


            //            Pis = d.Element("imposto")?.Element("PIS")?.Elements().Select(pis =>
            //            {
            //                var pisObj = new Pis();

            //                // PISAliq
            //                var pisAliq = pis.Element("PISAliq");
            //                if (pisAliq != null)
            //                {
            //                    pisObj.CST = pisAliq.Element("CST")?.Value;
            //                    pisObj.VBC = decimal.Parse(pisAliq.Element("vBC")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    pisObj.PPIS = decimal.Parse(pisAliq.Element("pPIS")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    pisObj.VPIS = decimal.Parse(pisAliq.Element("vPIS")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    return pisObj;
            //                }

            //                // PISQtde
            //                var pisQtde = pis.Element("PISQtde");
            //                if (pisQtde != null)
            //                {
            //                    pisObj.CST = pisQtde.Element("CST")?.Value;
            //                    pisObj.QBCProd = decimal.Parse(pisQtde.Element("qBCProd")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    pisObj.VAliqProd = decimal.Parse(pisQtde.Element("vAliqProd")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    pisObj.VPIS = decimal.Parse(pisQtde.Element("vPIS")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    return pisObj;
            //                }

            //                // PISNT
            //                var pisNT = pis.Element("PISNT");
            //                if (pisNT != null)
            //                {
            //                    pisObj.CST = pisNT.Element("CST")?.Value;
            //                    return pisObj;
            //                }

            //                // PISOutr
            //                var pisOutr = pis.Element("PISOutr");
            //                if (pisOutr != null)
            //                {
            //                    pisObj.CST = pisOutr.Element("CST")?.Value;
            //                    pisObj.VBC = pisOutr.Element("vBC") != null ? decimal.Parse(pisOutr.Element("vBC")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            //                    pisObj.PPIS = pisOutr.Element("pPIS") != null ? decimal.Parse(pisOutr.Element("pPIS")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            //                    pisObj.VPIS = pisOutr.Element("vPIS") != null ? decimal.Parse(pisOutr.Element("vPIS")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            //                    pisObj.QBCProd = pisOutr.Element("qBCProd") != null ? decimal.Parse(pisOutr.Element("qBCProd")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            //                    pisObj.VAliqProd = pisOutr.Element("vAliqProd") != null ? decimal.Parse(pisOutr.Element("vAliqProd")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            //                    return pisObj;
            //                }

            //                return null;
            //            }).FirstOrDefault(),


            //            Cofins = d.Element("imposto")?.Element("COFINS")?.Elements().Select(cofins =>
            //            {
            //                var cofinsObj = new Cofins();

            //                // COFINSAliq
            //                var cofinsAliq = cofins.Element("COFINSAliq");
            //                if (cofinsAliq != null)
            //                {
            //                    cofinsObj.CST = cofinsAliq.Element("CST")?.Value;
            //                    cofinsObj.VBC = decimal.Parse(cofinsAliq.Element("vBC")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    cofinsObj.PCOFINS = decimal.Parse(cofinsAliq.Element("pCOFINS")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    cofinsObj.VCOFINS = decimal.Parse(cofinsAliq.Element("vCOFINS")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    return cofinsObj;
            //                }

            //                // COFINSQtde
            //                var cofinsQtde = cofins.Element("COFINSQtde");
            //                if (cofinsQtde != null)
            //                {
            //                    cofinsObj.CST = cofinsQtde.Element("CST")?.Value;
            //                    cofinsObj.QBCProd = decimal.Parse(cofinsQtde.Element("qBCProd")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    cofinsObj.VAliqProd = decimal.Parse(cofinsQtde.Element("vAliqProd")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    cofinsObj.VCOFINS = decimal.Parse(cofinsQtde.Element("vCOFINS")?.Value ?? "0", CultureInfo.InvariantCulture);
            //                    return cofinsObj;
            //                }

            //                // COFINSNT
            //                var cofinsNT = cofins.Element("COFINSNT");
            //                if (cofinsNT != null)
            //                {
            //                    cofinsObj.CST = cofinsNT.Element("CST")?.Value;
            //                    return cofinsObj;
            //                }

            //                // COFINSOutr
            //                var cofinsOutr = cofins.Element("COFINSOutr");
            //                if (cofinsOutr != null)
            //                {
            //                    cofinsObj.CST = cofinsOutr.Element("CST")?.Value;
            //                    cofinsObj.VBC = cofinsOutr.Element("vBC") != null ? decimal.Parse(cofinsOutr.Element("vBC")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            //                    cofinsObj.PCOFINS = cofinsOutr.Element("pCOFINS") != null ? decimal.Parse(cofinsOutr.Element("pCOFINS")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            //                    cofinsObj.VCOFINS = cofinsOutr.Element("vCOFINS") != null ? decimal.Parse(cofinsOutr.Element("vCOFINS")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            //                    cofinsObj.QBCProd = cofinsOutr.Element("qBCProd") != null ? decimal.Parse(cofinsOutr.Element("qBCProd")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            //                    cofinsObj.VAliqProd = cofinsOutr.Element("vAliqProd") != null ? decimal.Parse(cofinsOutr.Element("vAliqProd")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            //                    return cofinsObj;
            //                }

            //                return null;
            //            }).FirstOrDefault(),

            //        }).ToList(),


            //        // Continue com Destinatario, Produtos, ICMSTot, e Transp
            //    };
            //}
            //return null;
        }
    }

}
