namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMAE_Amenazas")]
    public class Amenazas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public int? ID_Usu_alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }

        public Boolean es_activo { get; set; }

        public DateTime? fech_activo { get; set; }
        
    }
}
