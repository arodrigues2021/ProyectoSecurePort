namespace SecurePort.Entities.Models
{
    #region using
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    #endregion

    [Table("SPCON_Usuarios")]
    public class Usuarios
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string login { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string nombre { get; set; }

        [Required]
        public string apellidos { get; set; }
        
        public string email { get; set; }

        public Boolean? es_opip { get; set; }

        public DateTime? fech_exp_opip { get; set; }

        public Boolean? es_opp { get; set; }

        public int? categoria { get; set; }

        public int? id_grupo { get; set; }

        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        public Boolean? es_activo { get; set; }

        public DateTime fech_activo { get; set; }

        public Boolean? aviso_legal { get; set; }

        public string observaciones { get; set; }

        public string empresa { get; set; }

        [Required]
        public DateTime Fech_Password { get; set; }
 
    }
}
