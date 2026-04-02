namespace ControleFiscal.Utils
{
    public class StringUtils
    {
        public static string NormalizePropName(string name)
        {
            name = name.Trim();
            if (name.Length == 0) return name;
            return char.ToUpperInvariant(name[0]) + name.Substring(1);
        }

    }
}
