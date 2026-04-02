using System.Globalization;

namespace ControleFiscal.Utils
{
    public class ConverterHelper
    {
        public static string FormatarRealBrasileiro(decimal valor)
        {
            CultureInfo culturaPersonalizada = new CultureInfo("pt-BR");
            culturaPersonalizada.NumberFormat.CurrencySymbol = "R$";
            culturaPersonalizada.NumberFormat.CurrencyDecimalDigits = 2;
            culturaPersonalizada.NumberFormat.CurrencyDecimalSeparator = ",";
            culturaPersonalizada.NumberFormat.CurrencyGroupSeparator = ".";

            return valor.ToString("C", culturaPersonalizada);
        }


    }
}
