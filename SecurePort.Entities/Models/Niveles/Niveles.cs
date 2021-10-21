namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Niveles")]
    public class Niveles
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Puerto { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public string Emisor { get; set; }

        public string Receptor { get; set; }

        [Required]
        public int Nivel { get; set; }

        public string Emisor_Cancela { get; set; }

        public int? Duracion { get; set; }
         
        public DateTime? Fecha_Cancela { get; set; }

        public int? Nivel_Max { get; set; }

        public string Relevante { get; set; }

        public string Recomendacion { get; set; }

        public string Observaciones { get; set; }

        public int? ID_Usu_Alta { get; set; }

        public DateTime Fech_Alta { get; set; }

    }
}
