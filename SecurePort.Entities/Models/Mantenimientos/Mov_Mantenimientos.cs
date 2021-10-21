namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Mantenimientos")]
    public class Mantenimientos
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int id_Puerto { get; set; }

        public int? id_IIPP { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public DateTime fecha { get; set; }

        [Required]
        public string Equipo { get; set; }

        public string Realizador { get; set; }

        public string Validador { get; set; }

        public DateTime? fecha_Revision { get; set; }
        
        public string Observaciones { get; set; }

        public int? id_Usu_Alta { get; set; }

        [Required]
        public DateTime Fech_alta { get; set; }

    }
}
