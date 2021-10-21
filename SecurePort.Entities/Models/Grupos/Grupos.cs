namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPCON_Grupos")]
    public class Grupos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        public string Nombre { get; set; }

        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime? fech_alta { get; set; }

        [Required]
        public Boolean? es_activo { get; set; }

        public DateTime? fech_activo { get; set; }

        public string descripcion { get; set; }

        public int? Id_publico { get; set; }

    }
}
