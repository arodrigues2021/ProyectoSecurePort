namespace SecurePort.Entities.Models.Json
{
    using System;

    public class DeclaraMaritimaJson
    {
        public int Id { get; set; }

        public int Id_Organismo { get; set; }

        public int Id_Puerto { get; set; }

        public int Id_IIPP { get; set; }

        public string IMO { get; set; }

        public string Buque { get; set; }

        public int Id_Motivo { get; set; }

        public DateTime Fech_Expe { get; set; }

        public DateTime Fech_Ini_Validez { get; set; }

        public DateTime Fech_Fin_Validez { get; set; }

        public string Responsable { get; set; }

        public int? Nivel_Buq { get; set; }

        public int? Nivel_IIPP { get; set; }

        public string Medidas { get; set; }

        public string Observaciones { get; set; }

        public string action { get; set; }

        public string Validar { get; set; }
    }
}


