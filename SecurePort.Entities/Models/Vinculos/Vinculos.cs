namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMOV_Vinculos_SP")]
    public class Vinculos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; }

        public int? Id_Categoria { get; set; }

        [Required]
        public string Nombre { get; set; }
       
        [Required]
        public string Descripcion { get; set; }

        public int? Orden { get; set; }

    }
}
