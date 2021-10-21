namespace SecurePort.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SPMOV_Comites")]
    public class Comites
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int Id_Organismo { get; set; }

        [Required]
        public int Id_Puerto { get; set; }

        [Required]
        public int Nivel { get; set; }

        public string Observaciones { get; set; }

        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        [Required]
        public DateTime Convocado { get; set; }
    }
}
