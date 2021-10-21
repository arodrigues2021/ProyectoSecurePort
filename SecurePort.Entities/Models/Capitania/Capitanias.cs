namespace SecurePort.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SPMAE_Capitanias_Maritimas")]
    public class Capitanias
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string nombre { get; set; }
        
        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        [Required]
        public Boolean es_activo { get; set; }

        [Required]
        public DateTime fech_activo { get; set; }
        
    }
}
