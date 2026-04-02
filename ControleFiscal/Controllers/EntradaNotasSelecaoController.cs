using ControleFiscal.Infrastructure.Sql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Infrastructure.Sql.Focus.Context;


namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntradaNotasSelecaoController : ControllerBase
    {

        private readonly ILogger<EntradaNotasSelecaoController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public EntradaNotasSelecaoController(ILogger<EntradaNotasSelecaoController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextoLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextoLocal;
        }

        [HttpGet]
        public ActionResult<List<EntradaNotasSelecaoDTO>> Obter([FromQuery] int lojaId,  
                                                                [FromQuery] string inicio,
                                                                [FromQuery] string fim, 
                                                                [FromQuery] int fornecedorId = 0)
        {
            try
            {
                var nomeLoja = _Context.ConexaoCliente(lojaId , _ContextLocal);

                var sql = " SELECT E.CD_NOTA AS CDNOTA, E.DT_ENTRADA as DtEntrada, E.CNPJ, E.NM_FORNECEDOR as NmFornecedor, E.NUM_DOCUMENTO as NumDocumento, E.SERIE,E.VALOR_TOTAL_NOTA as ValorTotalNota" +
                          " FROM ENTRADA E WHERE COALESCE(E.DELETADO, 'F') <> 'V' AND DT_ENTRADA BETWEEN {0} AND {1} ";

                if (fornecedorId > 0)
                {
                    sql = sql + " CD_FORNECEDOR = " + fornecedorId;
                }

                DateTime dataInicio = new DateTime(DateTime.Parse(inicio).Year, DateTime.Parse(inicio).Month, DateTime.Parse(inicio).Day, 0, 0, 1);
                DateTime dataFim = new DateTime(DateTime.Parse(fim).Year, DateTime.Parse(fim).Month, DateTime.Parse(fim).Day, 23, 59, 59);

                var retorno = _Context.EntradaNotasSelecao.FromSqlRaw(sql,inicio,fim);
                return Ok(retorno);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }


        } 

    }
}