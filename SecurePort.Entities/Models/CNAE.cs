using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SecurePort.Entities.Models
{
    public class CNAE
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string IdCodigo { get; set; }

        public string Descripcion { get; set; }

    }
}