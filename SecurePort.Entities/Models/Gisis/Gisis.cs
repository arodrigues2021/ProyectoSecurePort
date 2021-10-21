namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Registro_GISIS")]
    public class RegistroGisis
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        [Required]
        public int Tipo_Registro { get; set; }

        public DateTime? Fech_Registro { get; set; }

        public string Motivo { get; set; }

        public int? ID_Usu_alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }


        
        
    }
}
