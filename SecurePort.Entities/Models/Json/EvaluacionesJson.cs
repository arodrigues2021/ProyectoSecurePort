using System;
namespace SecurePort.Entities.Models.Json
{
    public class EvaluacionesJson
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int Id_Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        public int Version { get; set; }

        public int Estado { get; set; }

        public string Responsable { get; set; }

        public string Cargo { get; set; }

        public DateTime? Fech_Cierre { get; set; }

        public DateTime? Fech_Procesada { get; set; }

        public DateTime? Fech_Conforme { get; set; }

        public DateTime? Fech_Tramitada { get; set; }

        public DateTime? Fech_Aprobada { get; set; }

        public DateTime? Fech_Rechazada { get; set; }

        public DateTime? Fech_NoConforme { get; set; }

        public DateTime? Fech_NoTramitada { get; set; }

        public DateTime? Fech_Zonificacion { get; set; }

        public DateTime? Fech_Info_Eval { get; set; }

        public DateTime? Fech_Info_Vune { get; set; }

        public string Observaciones { get; set; }

        public string action { get; set; }
    }
}


