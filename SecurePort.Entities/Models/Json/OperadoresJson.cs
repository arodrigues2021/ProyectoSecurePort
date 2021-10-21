namespace SecurePort.Entities.Models.Json
{
    public class OperadoresJson
    {
        public int Id { get; set; }

        public int Id_Instalacion { get; set; }

        public string Nombre { get; set; }

        public bool Es_Suplente { get; set; }

        public string Direccion { get; set; }

        public int? Id_Ciudad { get; set; }

        public int? Cod_postal { get; set; }

        public int? Id_provincia { get; set; }

        public string Telefono1 { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public bool Es_Activo { get; set; }

        public string Observaciones { get; set; }

        public string action { get; set; }

        public string ValidadoTipo { get; set; }

        public string cargo { get; set; }
    }
}


