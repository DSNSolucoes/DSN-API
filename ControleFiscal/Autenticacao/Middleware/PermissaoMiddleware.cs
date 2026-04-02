namespace ControleFiscal.Autenticacao.Middleware
{
    public class PermissaoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _permissoesRequeridas;

        public PermissaoMiddleware(RequestDelegate next, string[] permissoesRequeridas)
        {
            _next = next;
            _permissoesRequeridas = permissoesRequeridas;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity!.IsAuthenticated)
            {
                var userClaims = context.User.Claims;
                var permissions = userClaims.FirstOrDefault(c => c.Type == "permissions")?.Value.Split(',');

                if (permissions != null && _permissoesRequeridas.All(perm => permissions.Contains(perm)))
                {
                    await _next(context);
                    return;
                }

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Permissão negada");
                return;
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Não autenticado");
        }
    }

}
