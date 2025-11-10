using ClienteApiT.Data;
using ClienteApiT.Models;

namespace ClienteApiT.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, AppDbContext db)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error global");

                // Guardar en LogApi
                var log = new LogApi
                {
                    DateTime = DateTime.UtcNow,
                    TipoLog = "Error",
                    UrlEndpoint = context.Request.Path,
                    MetodoHttp = context.Request.Method,
                    RequestBody = "", // opcional: se puede leer body
                    ResponseBody = ex.Message,
                    DireccionIp = context.Connection.RemoteIpAddress?.ToString(),
                    Detalle = ex.ToString()
                };
                db.Logs.Add(log);
                await db.SaveChangesAsync();

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { Error = "Ocurrió un error interno." });
            }
        }
    }

}
