namespace SecurePort.Entities.Models.Json
{

    public class UploadJson
    {
        public string tipo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int? usuarioAlta { get; set; }
        public int servicioAlta { get; set; }
        public int id_servicio { get; set; }
        public string nombre_servicio { get; set; }

    }
}