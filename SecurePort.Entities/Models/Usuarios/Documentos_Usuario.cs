namespace SecurePort.Entities.Models
{
    #region using
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    #endregion

    [Table("SPMOV_Documentos_SP")]
    public class Docs_Usuario
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int id_servicio { get; set; }

        [Required]
        public int id_tipo_serv { get; set; }

        [Required]
        public int id_tipo_doc { get; set; }

        [Required]
        public string documento { get; set; }

        [Required]
        public string descripcion { get; set; }

        [Required]
        public DateTime fech_documento { get; set; }

        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

    }
}
