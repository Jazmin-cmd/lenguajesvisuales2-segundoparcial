using Microsoft.AspNetCore.Mvc;

namespace ClienteApiT.DTOS
{
    public class UploadZipFormModel
    {
        [FromForm(Name = "ciCliente")]
        public string CiCliente { get; set; }

        [FromForm(Name = "zipFile")]
        public IFormFile ZipFile { get; set; }
    }
}
