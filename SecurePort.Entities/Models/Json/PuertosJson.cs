namespace SecurePort.Entities.Models.Json
{
    public class PuertosJson
    {
        public int Id               { get; set; }
        public string Nombre        { get; set; }
        public int id_Organismo     { get; set; }
        public string Responsable   { get; set; }
        public string Direccion     { get; set; }
        public int? Id_Ciudad       { get; set; }
        public int? Cod_Postal      { get; set; }
        public int? Id_Provincia    { get; set; }
        public string Observaciones { get; set; }
        public int? Latitud         { get; set; }
        public int? Longitud        { get; set; }
        public string Locode        { get; set; }
        public int? Id_CapMarit     { get; set; }
        public string action        { get; set; }
        public string activo        { get; set; }
    }
}


