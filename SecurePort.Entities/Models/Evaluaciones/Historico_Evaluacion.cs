namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPEVA_Historico_Evaluacion")]
    public class Historico_Evaluacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Evaluacion { get; set; }

        [Required]
        public int Id_Version_Eval { get; set; }

        public string Accion { get; set; }

        [Required]
        public string Observaciones { get; set; }

        public int? ID_Usu_Accion { get; set; }
                
        public DateTime? Fech_Accion { get; set; }

    }
}
