namespace SecurePort.Entities.Models
{
    #region using
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    #endregion

    [Table("SPCON_Tipos_Documento")]
    public class Tipos_Documento
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string documento { get; set; }

        [Required]
        public string descripcion { get; set; }

        public string acceso { get; set; }
    }

}
