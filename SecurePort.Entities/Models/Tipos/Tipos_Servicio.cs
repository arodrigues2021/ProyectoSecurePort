namespace SecurePort.Entities.Models
{
    #region using
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    #endregion
    
    [Table("SPCON_Tipos_Servicio")]
    public class Tipos_Servicio
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string servicio { get; set; }

        [Required]
        public string descripcion { get; set; }
        
    }

}

