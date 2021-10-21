namespace SecurePort.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SPMAE_Ciudades")]
    public class Ciudades
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string codigo { get; set; }

        [Required]
        public string nombre { get; set; }

        [Required]
        public int Id_provincia { get; set; }
       
        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        [Required]
        public Boolean es_activo { get; set; }

        [Required]
        public DateTime fech_activo { get; set; }

        public int? Id_isla { get; set; }
    }
}
