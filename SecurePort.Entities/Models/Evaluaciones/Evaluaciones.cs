namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPEVA_Evaluaciones")]
    public class Evaluaciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public int Id_Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        public int? ID_Usu_alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }

    }
}
