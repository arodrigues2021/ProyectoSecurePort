namespace SecurePort.Entities.Models.Json
{
    public class BienesJson
    {
        public int Id               { get; set; }
        public string Descripcion   { get; set; }
        public int id_Tipo_IIPP     { get; set; }
        public int? id_Bien_Padre    { get; set; }
        public string action        { get; set; }
        public string activo        { get; set; }
    }
}


