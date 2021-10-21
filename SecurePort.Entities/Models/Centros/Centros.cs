namespace SecurePort.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SPMOV_Centros_24H")]
    public class Centros
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int Id_Organismo { get; set; }

        public int? Id_Puerto { get; set; }

        [Required]
        public string Centro_24H { get; set; }

        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        public string Direccion { get; set; }

        public int? Id_Ciudad { get; set; }

        public int? Cod_Postal { get; set; }

        public int? Id_Provincia { get; set; }

    }
}
