namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Detalle_Instalacion")]
    public class MOV_Detalle_Instalacion
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int ID_Instalacion { get; set; }

        [Required]
        public int ID_Bien { get; set; }

        public int? ID_Amenaza { get; set; }
        
        public int? ID_Usu_Alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }

        
    }
}
