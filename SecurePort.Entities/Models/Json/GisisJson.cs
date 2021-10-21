using System;
namespace SecurePort.Entities.Models.Json
{
    public class GisisJson
    {
        public int Id { get; set; }

        public int Id_Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        public int Tipo_Registro { get; set; }

        public DateTime? Fecha_Registro { get; set; }

        public string Motivo { get; set; }

        public DateTime Fech_Activo { get; set; }

        public string action { get; set; }
    }
}


