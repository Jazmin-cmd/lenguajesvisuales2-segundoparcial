namespace ClienteApiT.Models
{
    public class LogApi
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string TipoLog { get; set; } = null!; // ERROR, INFO
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public string UrlEndpoint { get; set; } = null!;
        public string MetodoHttp { get; set; } = null!;
        public string? DireccionIp { get; set; }
        public string? Detalle { get; set; }
    }

}
