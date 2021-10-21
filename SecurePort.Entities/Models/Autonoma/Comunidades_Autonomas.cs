namespace SecurePort.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SPMAE_Comunidades_Autonomas")]
    public class Comunidades_Autonomas
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public int ID_Pais{ get; set; }

        public int? ID_Usu_Alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

        [Required]
        public DateTime fech_activo { get; set; }

        [Required]
        public Boolean es_activo { get; set; }

        
        

    }

}
