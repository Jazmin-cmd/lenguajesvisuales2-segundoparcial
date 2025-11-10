using ClienteApiT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ArchivoCliente
{
    [Key]  // <-- Esto asegura que EF lo tome como PK
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdArchivo { get; set; }

    public int ClienteId { get; set; }  // FK hacia Cliente
    public Cliente Cliente { get; set; } = null!;

    public string NombreArchivo { get; set; } = null!;
    public string UrlArchivo { get; set; } = null!;
    public DateTime FechaSubida { get; set; }
}
