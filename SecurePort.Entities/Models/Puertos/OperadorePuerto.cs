namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMAE_Operadores_Puerto")]
    public class OperadoresPuerto
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_puerto { get; set; }

         [Required]
        public string Nombre { get; set; }

         [Required]
        public Boolean Es_Suplente { get; set; }

        public string Telefono { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Direccion { get; set; }

        public int? ID_Ciudad { get; set; }
        
        public int? Cod_Postal { get; set; }

        public int? ID_Provincia { get; set; }

        public int? Id_usu_alta { get; set; }

        [Required]
        public DateTime Fech_alta { get; set; }

        public Boolean Es_Activo { get; set; }
        
        public DateTime Fech_activo { get; set; }

        public string Observaciones { get; set; }



    }
}
