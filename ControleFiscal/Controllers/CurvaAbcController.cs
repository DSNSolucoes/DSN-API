using ControleFiscal.Context.NFe;
using ControleFiscal.Domain.DTO.Relatorio;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Focus.Context;
using ControleFiscal.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurvaAbcController : ControllerBase
    {
        private readonly ILogger<CurvaAbcController> _logger;
        private readonly ContextControleFiscalContext _context;
        private readonly ContextLocalContext _contextLocal;

        public CurvaAbcController(
            ILogger<CurvaAbcController> logger,
            ContextControleFiscalContext context,
            ContextLocalContext contextLocal)
        {
            _logger = logger;
            _context = context;
            _contextLocal = contextLocal;
        }

        [HttpGet]
        public ActionResult<List<CurvaAbcDTO>> Obter(
            [FromQuery] int lojaId,
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim,
            [FromQuery] string criterio = "V",
            [FromQuery] decimal percentualA = 70,
            [FromQuery] decimal percentualB = 20)
        {
            var nomeLoja = "";
            try
            {
                if (lojaId <= 0)
                    return BadRequest("LojaId é obrigatório.");

                if (dataFim < dataInicio)
                    return BadRequest("Data fim deve ser maior ou igual à data início.");

                if (percentualA <= 0 || percentualB <= 0 || percentualA + percentualB >= 100)
                    return BadRequest("Percentuais inválidos. A soma de A e B deve ser menor que 100.");

                var loja = _contextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);
                if (loja == null) return BadRequest("Loja não encontrada.");

                nomeLoja = loja.Nome;
                _context.ConexaoCliente(loja.Caminho, loja.Host);

                var inicio = dataInicio.Date;
                var fim = dataFim.AddDays(1).AddSeconds(-1);

                // Use objetos DateTime, o EF cuidará da conversão para o Firebird
                var rows = (from pi in _context.PedidoItem
                            join pe in _context.Pedido on pi.NumDocumento equals pe.NumDocumento
                            join p in _context.Produtos on pi.CdProduto equals p.CdProduto into produtosJoin
                            from p in produtosJoin.DefaultIfEmpty() // Isso faz o LEFT JOIN
                            where
                                (pe.Cancelado ?? "F") != "V" &&
                                (pi.Cancelado ?? "F") != "V" &&
                                pe.DtFechamento >= inicio &&
                                pe.DtFechamento <= fim &&
                                pi.CdProduto != null
                            group new { pi, p } by pi.CdProduto into g
                            where g.Sum(x => x.pi.Subtotal) > 0 // Isso é o HAVING
                            select new CurvaAbcDTO
                            {
                                CdProduto = g.Key,
                                NmProduto = g.Max(x => x.p.NmProduto),
                                CodBarras = g.Max(x => x.p.CodBarras),
                                QuantidadeVendida = g.Sum(x => x.pi.Quantidade),
                                ValorVendido = g.Sum(x => x.pi.Subtotal)
                            }).ToList();

                if (!rows.Any())
                    return Ok(new List<CurvaAbcDTO>());

                var totalGeral = criterio == "Q"
                    ? rows.Sum(r => r.QuantidadeVendida)
                    : rows.Sum(r => r.ValorVendido);

                if (totalGeral == 0)
                    return Ok(new List<CurvaAbcDTO>());

                var ordenados = criterio == "Q"
                    ? rows.OrderByDescending(r => r.QuantidadeVendida).ToList()
                    : rows.OrderByDescending(r => r.ValorVendido).ToList();

                var limiteA = percentualA;
                var limiteB = percentualA + percentualB;

                var acumulado = 0m;
                var resultado = new List<CurvaAbcDTO>();

                for (int i = 0; i < ordenados.Count; i++)
                {
                    var row = ordenados[i];
                    var valorCriterio = criterio == "Q" ? row.QuantidadeVendida : row.ValorVendido;
                    var percIndividual = Math.Round(valorCriterio.GetValueOrDefault(0) / totalGeral.GetValueOrDefault(0) * 100, 4);
                    acumulado += percIndividual;

                    string classe;
                    if (acumulado <= limiteA + 0.001m)      classe = "A";
                    else if (acumulado <= limiteB + 0.001m) classe = "B";
                    else                                    classe = "C";

                    resultado.Add(new CurvaAbcDTO
                    {
                        Posicao = i + 1,
                        CdProduto = row.CdProduto,
                        CodBarras = row.CodBarras,
                        NmProduto = row.NmProduto,
                        QuantidadeVendida = row.QuantidadeVendida,
                        ValorVendido = row.ValorVendido,
                        PercentualIndividual = percIndividual,
                        PercentualAcumulado = Math.Round(acumulado, 2),
                        Classe = classe
                    });
                }

                return Ok(resultado);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao calcular curva ABC.");
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }
        }
    }
}
