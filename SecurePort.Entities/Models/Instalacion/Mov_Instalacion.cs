namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Instalaciones")]
    public class MOV_Instalaciones
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public int? id_Puerto { get; set; }

        [Required]
        public int? id_Tipo { get; set; }

        [Required]
        public int es_concesionada { get; set; }

        public string Empresa { get; set; }

        [Required]
        public int Clasificacion { get; set; }

        public string Direccion { get; set; }

        public int? id_Ciudad { get; set; }
        
        public int? cod_postal { get; set; }

        public int? id_provincia { get; set; }

        public string OMI { get; set; }

        [Required]
        public string Nivel { get; set; }

        public int? Longitud { get; set; }

        public int? Latitud { get; set; }

        public Boolean Declara_Cumpli { get; set; }

        public DateTime? Fech_Declara_Cumpli  { get; set; }

        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        [Required]
        public DateTime fech_Activo { get; set; }

        [Required]
        public Boolean Es_Activo { get; set; }

        public string Observaciones { get; set; }

    }
}
