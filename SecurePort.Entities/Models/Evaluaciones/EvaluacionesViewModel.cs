namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class EvaluacionesViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int Id_Puerto { get; set; }

        public string Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        public string IIPP { get; set; }

        public int Version { get; set; }

        public int Estado { get; set; }

        public string NombreEstado { get; set; }

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

        public DateTime Fech_Alta { get; set; }


    }
}
