namespace SecurePort.Entities.Models
{
    #region using
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPCON_Grupos_Acciones")]
    public class Gestionpermisos
    {
        [Key]
        public int Id { get; set; }
       
        [Required]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

    }
}
