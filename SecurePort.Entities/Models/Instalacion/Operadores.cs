namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Operadores_Instalacion")]
    public class Operadores
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Instalacion { get; set; }

        public string Nombre { get; set; }
           
        public Boolean Es_Suplente { get; set; }

        public string Direccion { get; set; }

        public int? Id_Ciudad { get; set; }
        
        public int? Cod_postal { get; set; }

        public int? Id_provincia { get; set; }

        public string Telefono1 { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public int? Id_usu_alta { get; set; }

        [Required]
        public DateTime Fech_alta { get; set; }

        public Boolean Es_Activo { get; set; }
        
        public DateTime Fech_activo { get; set; }

        public string Observaciones { get; set; }

        public string Cargo { get; set; }

    }
}
