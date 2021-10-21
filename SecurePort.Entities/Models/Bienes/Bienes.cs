namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMAE_Bienes")]
    public class Bienes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public int? id_Tipo_IIPP { get; set; }

        public int? id_Bien_Padre { get; set; }

        public int? ID_Usu_alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }

        public Boolean es_activo { get; set; }

        public DateTime? fech_activo { get; set; }
        
    }
}
