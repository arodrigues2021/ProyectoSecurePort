namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Procedimientos")]
    public class Procedimientos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? Id_Organismo { get; set; }

        public bool? Ind_AP { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public string Observaciones { get; set; }

        public int? ID_Usu_alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }
        
        
    }
}
