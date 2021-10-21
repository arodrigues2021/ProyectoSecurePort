namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Formaciones")]
    public class Formaciones
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Organismo { get; set; }

        [Required]
        public int Id_Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        public string Titulo { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fin { get; set; }

        public int Duracion { get; set; }

        public string Lugar { get; set; }

        public string Entidad { get; set; }

        public string Observaciones { get; set; }

        public int? ID_Usu_Alta { get; set; }

        public DateTime Fech_Alta { get; set; }

    }
}
