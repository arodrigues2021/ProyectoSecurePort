namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPEVA_Versiones_Evaluacion")]
    public class Versiones_Evaluacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Evaluacion { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        public int Estado { get; set; }

        [Required]
        public string Responsable { get; set; }

        [Required]
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

        public int? ID_Usu_alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }

    }
}
