using ControleFiscal.Infrastructure.Sql; 
using ControleFiscal.Infrastructure.Sql.Focus.Context;
using Microsoft.AspNetCore.Mvc;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FallBackController : ControllerBase
    {

        private readonly ILogger<FallBackController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public FallBackController(ILogger<FallBackController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/html");
        }

         
    }
}