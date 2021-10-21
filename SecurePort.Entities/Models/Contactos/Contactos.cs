namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMAE_Contactos_Organismo")]
    public class Contactos_Organismo
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Organismo { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public Boolean Es_Responsable { get; set; }

        public string Telefono { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public int? ID_Usu_Alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        [Required]
        public Boolean es_activo { get; set; }

        [Required]
        public DateTime fech_activo { get; set; }

        public string Observaciones { get; set; }

        public string Cargo { get; set; }


    }
}
