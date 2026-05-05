using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace ControleFiscal.Utils
{
    public class ConfiguracaoHelper
    {
        private static IConfigurationRoot configuration;

        static ConfiguracaoHelper()
        {
            // Construir a configuração usando appsettings.json
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Define o diretório base para a configuração
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Carrega o arquivo appsettings.json
                .Build(); // Constroi o objeto IConfigurationRoot
        }
        public static void Gravar(string objeto, string chave, string valor)
        {
            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            var json = File.ReadAllText(appSettingsPath);

            if (json != null)
            {
                dynamic? jsonObj = JsonConvert.DeserializeObject(json);

                if (jsonObj != null)
                {
                    jsonObj[objeto][chave] = valor;

                    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(appSettingsPath, output);
                }
            }
        }

        public static string Ler(string objeto, string chave )
        { 
            var retorno =  configuration[objeto + ":" + chave];

            return retorno is null ? string.Empty : retorno;
        }
    }
}
