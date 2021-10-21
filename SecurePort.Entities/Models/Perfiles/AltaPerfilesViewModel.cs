namespace SecurePort.Entities.Models
{
    #region using
    using System.ComponentModel.DataAnnotations;
    #endregion

    public class AltaPerfilesViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public bool alta { get; set; }

        public bool asignar { get; set; }
    }
}
