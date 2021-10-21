namespace SecurePort.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SPMOV_Comunica_Centro24h")]
    public class Comunica_Centro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int Id_Centro24h { get; set; }

        [Required]
        public int Tipo_Canal { get; set; }

        [Required]
        public string Dato { get; set; }

        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        public string Nota { get; set; }

    }
}
