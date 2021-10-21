namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Comunicaciones")]
    public class Comunicaciones
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        [Required]
        public string Asunto { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Emisor { get; set; }

        [Required]
        public string Receptor { get; set; }

        public string Mensaje { get; set; }

        [Required]
        public bool Recibido { get; set; }
      
        public int? Id_Usu_Alta { get; set; }

        [Required]
        public DateTime Fech_alta { get; set; }

    }
}
