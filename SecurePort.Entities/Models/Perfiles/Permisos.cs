namespace SecurePort.Entities.Models
{
    #region using
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPCON_Acciones")]
    public class Acciones
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string descripcion { get; set; }

        [Required]
        public int id_grupo_accion { get; set; }

        public int? orden { get; set; }
    }
}
