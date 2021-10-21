namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Incidentes")]
    public class Incidentes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Puerto { get; set; }
       
        [Required]
        public string Descripcion { get; set; }

        [Required]
        public int Tipo { get; set; }

        [Required]
        public DateTime Fecha { get; set; }
        
        public string Observaciones { get; set; }

        public int? ID_Usu_alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }

       
    }
}
