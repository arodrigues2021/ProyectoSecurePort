namespace SecurePort.Entities.Models
{
    public class Documento
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int tipodocumento { get; set; }
        public string descripcion { get; set; }
        public string documento { get; set; }
    }
}

