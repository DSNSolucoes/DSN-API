using System.Xml;
using System.Xml.Serialization;

namespace ControleFiscal.Controllers
{
    public class NFeSerialization
    {
        public T? GetObjectFromFile<T>(string filePath) where T : class
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                Console.Error.WriteLine($"Arquivo inválido: {filePath}");
                return null;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var reader = XmlReader.Create(filePath);
                return serializer.Deserialize(reader) as T;
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine($"Erro de desserialização: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro inesperado: {ex.Message}");
                return null;
            }
        }

        public T? GetObjectFromFile<T>(Stream filePath) where T : class
        { 

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var reader = XmlReader.Create(filePath);
                return serializer.Deserialize(reader) as T;
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine($"Erro de desserialização: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro inesperado: {ex.Message}");
                return null;
            }
        }
    }


}
