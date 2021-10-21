namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMAE_Organismos")]
    public class Organismos
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public int Tipo { get; set; }

        public int? ID_Usu_Alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        [Required]
        public Boolean es_activo { get; set; }

        [Required]
        public DateTime fech_activo { get; set; }

        public string Observaciones { get; set; }

        [Required]
        public int ID_Ciudad { get; set; }

        [Required]
        public int Cod_Postal { get; set; }

        [Required]
        public int ID_Provincia { get; set; }

        [Required]
        public string Direccion { get; set; }


    }
}
