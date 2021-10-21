namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPCON_Perfiles_Grupo")]
    public class PerfilesGrupo
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int Id_grupo { get; set; }

        [Required]
        public int Id_perfil { get; set; }

        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime? fech_alta { get; set; }

    }
}
