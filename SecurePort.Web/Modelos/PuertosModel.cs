using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebSecurePort.Models.Validacion;

namespace WebSecurePort.Models
{

    [MetadataType(typeof(PuertosMetadata))]
    public class Puertos
    {
        public int Id { get; set ; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    [Table("Puertos")]
    public class PuertosMetadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Display(Name = "Nombre", ResourceType = typeof(SecurePortResources.Resources))]
        [Required(ErrorMessageResourceName = "Name", ErrorMessageResourceType = typeof(SecurePortResources.Resources))]
        [MaxNumeroAtributo(5)]
        public string Name { get; set; }
        
        [Display(Name = "Telefono", ResourceType = typeof(SecurePortResources.Resources))]
        [MaxNumeroAtributo(10)]
        public string Phone { get; set; }

        [MaxNumeroAtributo(10)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(SecurePortResources.Resources))]
        public string Email { get; set; }

    }
    
}