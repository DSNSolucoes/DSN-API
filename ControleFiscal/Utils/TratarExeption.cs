using System.Text;

namespace ControleFiscal.Utils
{
    public class TratarExeption
    {


        public static string TratarMensagemUsuario(Exception e, string? loja)
        {
            if (e == null) { return ""; }

            if (e.Message.Contains("network"))
            {
                return "Não foi possivel se conectar ao banco de dados: " + loja;
            }


            if (e.Message.Contains("required column"))
            {
                var mensagem = new StringBuilder();
                mensagem.Append("Não foi possivel se conectar ao banco de dados: " + loja + "\n");
                mensagem.Append("Banco de Dados esta inconsistente." + e.Message);

                return mensagem.ToString();
            }


            return e.StackTrace;

        }
    }

}
