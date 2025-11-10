using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ClienteApiT.Models
{
    public class ClienteCreateFormModel
    {
        [Required]
        public string CI { get; set; } = null!;

        [Required]
        public string Nombres { get; set; } = null!;

        [Required]
        public string Direccion { get; set; } = null!;

        [Required]
        public string Telefono { get; set; } = null!;

        // Fotos opcionales
        public IFormFile? FotoCasa1 { get; set; }
        public IFormFile? FotoCasa2 { get; set; }
        public IFormFile? FotoCasa3 { get; set; }
    }
}
