using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSecurePort.Models
{

    [MetadataType(typeof(UsuariosMetadata))]
    public class Usuarios
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    [Table("Usuarios")]
    public class UsuariosMetadata
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}