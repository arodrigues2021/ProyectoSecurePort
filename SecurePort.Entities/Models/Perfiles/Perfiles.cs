namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPCON_Perfiles")]
    public class Perfiles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        [Required]
        public string Nombre { get; set; }

        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        public Boolean? es_activo { get; set; }

        public DateTime? fech_activo { get; set; }

        public string descripcion { get; set; }


    }
}
