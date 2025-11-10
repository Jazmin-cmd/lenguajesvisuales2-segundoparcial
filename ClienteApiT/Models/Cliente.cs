namespace ClienteApiT.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string CI { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;

        // Opcional: fotos como varbinary si querés guardar bytes
        public byte[]? FotoCasa1 { get; set; }
        public byte[]? FotoCasa2 { get; set; }
        public byte[]? FotoCasa3 { get; set; }

        // Nuevas propiedades para almacenar URLs de fotos
        public string? UrlFoto1 { get; set; }
        public string? UrlFoto2 { get; set; }
        public string? UrlFoto3 { get; set; }

        // Relación con archivos subidos
        public List<ArchivoCliente> Archivos { get; set; } = new List<ArchivoCliente>();
    }


}
