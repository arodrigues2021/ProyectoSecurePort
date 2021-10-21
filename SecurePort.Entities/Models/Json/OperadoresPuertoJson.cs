namespace SecurePort.Entities.Models.Json
{
    public class OperadoresPuertoJson
    {
        public int Id { get; set; }

        public int Id_Puerto { get; set; }

        public string Nombre { get; set; }

        public bool Es_Suplente { get; set; }

        public string Telefono { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public bool Es_Activo { get; set; }

        public string Observaciones { get; set; }

        public string action { get; set; }

        public string Direccion { get; set; }

        public int? ID_Ciudad { get; set; }

        public int? Cod_Postal { get; set; }

        public int? ID_Provincia { get; set; }

        public string ValidadoTipo { get; set; }

    }
}


