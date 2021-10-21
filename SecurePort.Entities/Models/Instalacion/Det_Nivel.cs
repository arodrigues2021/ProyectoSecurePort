namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Det_Nivel")]
    public class Det_Nivel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Nivel { get; set; }

        public int Id_IIPP { get; set; }

        public int? id_usu_alta { get; set; }

        [Required]
        public DateTime fech_alta { get; set; }

    }
}
