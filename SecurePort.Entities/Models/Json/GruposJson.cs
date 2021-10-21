namespace SecurePort.Entities.Models.Json
{
   
    public class GruposJson
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Description { get; set; }
        public int? Id_publico { get; set; }
        public string Activo { get; set; }
    }
}