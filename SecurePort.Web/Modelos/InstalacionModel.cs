using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebSecurePort.Models.Validacion;

namespace WebSecurePort.Models
{
    using System;

    [MetadataType(typeof(InstalacionMetadata))]
    public class Instalacion
    {
        public int IdInst  { get; set; }
        public string Name { get; set; }
        public int IdTipo  { get; set; }
        public int IdCNAE  { get; set; }
        public int Clasifi { get; set; }
        public string Contacto { get; set; }
        public string Phone { get; set; }
        public string otros { get; set; }
        public int IdUsuarioAlta { get; set; }
        public int IdUsuarioMod { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaMod { get; set; }
    }

    [Table("Instalacion")]
    public class InstalacionMetadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdInst { get; set; }

        [Display(Name = "Descripcion", ResourceType = typeof(SecurePortResources.Resources))]
        [Required(ErrorMessageResourceName = "Instalacion", ErrorMessageResourceType = typeof(SecurePortResources.Resources))]
        [MaxNumeroAtributo(50)]
        public string Name { get; set; }

        [Display(Name = "Instalacion", ResourceType = typeof(SecurePortResources.Resources))]
        [Required(ErrorMessageResourceName = "DescripcionInstalacion", ErrorMessageResourceType = typeof(SecurePortResources.Resources))]
        [MaxNumeroAtributo(4)]
        public int IdTipo { get; set; }

        [Display(Name = "CNAE", ResourceType = typeof(SecurePortResources.Resources))]
        [Required(ErrorMessageResourceName = "DescripcionCNAE", ErrorMessageResourceType = typeof(SecurePortResources.Resources))]
        [MaxNumeroAtributo(4)]
        public int IdCNAE { get; set; }

        [Display(Name = "Clasifica", ResourceType = typeof(SecurePortResources.Resources))]
        [Required(ErrorMessageResourceName = "DescripcionClasifica", ErrorMessageResourceType = typeof(SecurePortResources.Resources))]
        [MaxNumeroAtributo(2)]
        public int Clasifi { get; set; }

        [Display(Name = "Contacto", ResourceType = typeof(SecurePortResources.Resources))]
        [MaxNumeroAtributo(30)]
        public string Contacto { get; set; }
        
        [Display(Name = "Telefono", ResourceType = typeof(SecurePortResources.Resources))]
        [MaxNumeroAtributo(10)]
        public string Phone { get; set; }

        [Display(Name = "otros", ResourceType = typeof(SecurePortResources.Resources))]
        [MaxNumeroAtributo(10)]
        public string otros { get; set; }


    }
}



