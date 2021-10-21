namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Practicas")]
    public class Practicas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Puerto { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public int Tipo { get; set; }

        [Required]
        public int Estado { get; set; }

        public DateTime? Ini_Programa { get; set; }

        public DateTime? Fin_Programa { get; set; }

        public DateTime? Ini_Planifica { get; set; }

        public DateTime? Fin_Planifica { get; set; }

        public DateTime? Ini_Real { get; set; }

        public DateTime? Fin_Real { get; set; }

        public string Responsable { get; set; }

        public string Cuerpos { get; set; }

        public int Valoracion { get; set; }

        public bool Ratifico { get; set; }

        public string Conclusiones { get; set; }

        public string Propuesta { get; set; }

        public string Observaciones { get; set; }
        
        public int? ID_Usu_alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }
        
    }
}
