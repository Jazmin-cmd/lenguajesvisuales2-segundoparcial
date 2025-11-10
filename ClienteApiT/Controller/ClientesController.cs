using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClienteApiT.Models;
using ClienteApiT.Data;

namespace ClienteApiT.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(AppDbContext db, IWebHostEnvironment env, ILogger<ClientesController> logger)
        {
            _db = db;
            _env = env;
            _logger = logger;
        }

        // POST: api/clientes
        [HttpPost]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> Create([FromForm] ClienteCreateFormModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (await _db.Clientes.AnyAsync(c => c.CI == model.CI))
                    return Conflict("CI ya registrado.");

                var cliente = new Cliente
                {
                    CI = model.CI,
                    Nombres = model.Nombres,
                    Direccion = model.Direccion,
                    Telefono = model.Telefono
                };

                // Carpeta de subida: wwwroot/uploads/{CI}/
                var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", model.CI);
                Directory.CreateDirectory(uploads);

                // Función segura para subir archivos opcionales
                async Task<string?> SaveFile(IFormFile? file, string prefix)
                {
                    if (file == null || file.Length == 0)
                        return null;

                    var allowedExt = new[] { ".jpg", ".jpeg", ".png" };
                    var ext = Path.GetExtension(file.FileName)?.ToLower();
                    if (!allowedExt.Contains(ext))
                        throw new InvalidOperationException("Formato de foto no permitido.");

                    var safeName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploads, $"{prefix}_{Guid.NewGuid()}_{safeName}");

                    await using var stream = System.IO.File.Create(filePath);
                    await file.CopyToAsync(stream);

                    return Path.Combine("uploads", model.CI, Path.GetFileName(filePath)).Replace("\\", "/");
                }

                // Subir fotos solo si existen
                cliente.UrlFoto1 = await SaveFile(model.FotoCasa1, "foto1");
                cliente.UrlFoto2 = await SaveFile(model.FotoCasa2, "foto2");
                cliente.UrlFoto3 = await SaveFile(model.FotoCasa3, "foto3");

                _db.Clientes.Add(cliente);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, new
                {
                    cliente.Id,
                    cliente.CI,
                    cliente.Nombres,
                    cliente.Direccion,
                    cliente.Telefono,
                    cliente.UrlFoto1,
                    cliente.UrlFoto2,
                    cliente.UrlFoto3
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente");

                // Guardar en Logs
                var log = new LogApi
                {
                    DateTime = DateTime.UtcNow,
                    TipoLog = "Error",
                    UrlEndpoint = HttpContext.Request.Path,
                    MetodoHttp = HttpContext.Request.Method,
                    RequestBody = "",
                    ResponseBody = ex.Message,
                    DireccionIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Detalle = ex.ToString()
                };

                _db.Logs.Add(log);
                await _db.SaveChangesAsync();

                return StatusCode(500, "Ocurrió un error interno.");
            }
        }

        // GET: api/clientes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _db.Clientes.Include(c => c.Archivos)
                                            .FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null)
                return NotFound();

            return Ok(new
            {
                cliente.Id,
                cliente.CI,
                cliente.Nombres,
                cliente.Direccion,
                cliente.Telefono,
                cliente.UrlFoto1,
                cliente.UrlFoto2,
                cliente.UrlFoto3,
                Archivos = cliente.Archivos.Select(a => new { a.IdArchivo, a.NombreArchivo, a.UrlArchivo })
            });
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _db.Clientes.Include(c => c.Archivos).ToListAsync();
            var result = clientes.Select(cliente => new
            {
                cliente.Id,
                cliente.CI,
                cliente.Nombres,
                cliente.Direccion,
                cliente.Telefono,
                cliente.UrlFoto1,
                cliente.UrlFoto2,
                cliente.UrlFoto3,
                Archivos = cliente.Archivos.Select(a => new { a.IdArchivo, a.NombreArchivo, a.UrlArchivo })
            });
            return Ok(result);
        }
    }
}
