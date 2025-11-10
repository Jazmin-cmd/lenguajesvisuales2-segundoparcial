using ClienteApiT.Data;
using ClienteApiT.DTOS;
using ClienteApiT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace ClienteApiT.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FilesController> _logger;

        public FilesController(AppDbContext db, IWebHostEnvironment env, ILogger<FilesController> logger)
        {
            _db = db;
            _env = env;
            _logger = logger;
        }

        [HttpPost("uploadZip")]
        [RequestSizeLimit(200_000_000)]
        public async Task<IActionResult> UploadZip([FromForm] UploadZipFormModel model)
        {
            if (model.ZipFile == null || model.ZipFile.Length == 0)
                return BadRequest("Archivo zip vacío");

            var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.CI == model.CiCliente);
            if (cliente == null)
                return NotFound("Cliente no encontrado");

            var targetDir = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", model.CiCliente, "zip");
            Directory.CreateDirectory(targetDir);

            var tempZipPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_{model.ZipFile.FileName}");
            using (var fs = System.IO.File.Create(tempZipPath))
                await model.ZipFile.CopyToAsync(fs);

            try
            {
                using var archive = ZipFile.OpenRead(tempZipPath);
                var archivosSubidos = new List<object>();
                var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".mp4" };

                foreach (var entry in archive.Entries)
                {
                    if (string.IsNullOrEmpty(entry.Name)) continue;

                    var ext = Path.GetExtension(entry.Name).ToLower();
                    if (!allowedExt.Contains(ext)) continue;

                    var safeName = Path.GetFileName(entry.Name);
                    var destPath = Path.Combine(targetDir, $"{Guid.NewGuid()}_{safeName}");
                    entry.ExtractToFile(destPath);

                    var relativeUrl = Path.Combine("uploads", model.CiCliente, "zip", Path.GetFileName(destPath))
                                          .Replace("\\", "/");

                    var archivo = new ArchivoCliente
                    {
                        NombreArchivo = safeName,
                        UrlArchivo = relativeUrl,
                        ClienteId = cliente.Id,
                        FechaSubida = DateTime.UtcNow
                    };

                    _db.ArchivosClientes.Add(archivo);
                    archivosSubidos.Add(new { archivo.NombreArchivo, archivo.UrlArchivo });
                }

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    Mensaje = "Archivos subidos y registrados",
                    Archivos = archivosSubidos
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descomprimiendo zip");

                var log = new LogApi
                {
                    DateTime = DateTime.UtcNow,
                    TipoLog = "Error",
                    UrlEndpoint = HttpContext.Request.Path,
                    MetodoHttp = HttpContext.Request.Method,
                    RequestBody = model.CiCliente,
                    ResponseBody = ex.Message,
                    DireccionIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Detalle = ex.ToString()
                };

                _db.Logs.Add(log);
                await _db.SaveChangesAsync();

                return StatusCode(500, "Error al procesar zip");
            }
            finally
            {
                if (System.IO.File.Exists(tempZipPath))
                    System.IO.File.Delete(tempZipPath);
            }
        }

    }
}
