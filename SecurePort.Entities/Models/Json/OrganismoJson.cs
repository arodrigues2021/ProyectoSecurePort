namespace SecurePort.Entities.Models.Json
{
    public class OrganismoJson
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int Tipo { get; set; }

        public string Observaciones { get; set; }

        public int ID_Ciudad { get; set; }

        public int Cod_Postal { get; set; }

        public int ID_Provincia { get; set; }

        public string Direccion { get; set; }

        public string action { get; set; }

        public string activo { get; set; }
    }
}
