namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Declara_Maritima")]
    public class DeclaraMaritima
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_IIPP { get; set; }

        public string IMO { get; set; }

        public string Buque { get; set; }

        [Required]
        public int Id_Motivo { get; set; }
        
        [Required]
        public DateTime Fech_Expe { get; set; }

        public DateTime Fech_Ini_Validez { get; set; }

        public DateTime Fech_Fin_Validez { get; set; }

        public string Responsable { get; set; }

        public int? Nivel_Buq { get; set; }

        public int? Nivel_IIPP { get; set; }

        public string Medidas { get; set; }

        public string Observaciones { get; set; }

        public int? ID_Usu_alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }
        
    }
}
