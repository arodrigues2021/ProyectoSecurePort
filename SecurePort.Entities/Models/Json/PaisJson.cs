namespace SecurePort.Entities.Models.Json
{
    public class PaisJson
    {
        public int ID_pais { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public int ID_Provincia { get; set; }
        public string Tipo { get; set; }
        public string activo { get; set; }
        public int? ID_Isla { get; set; }

    }
}
