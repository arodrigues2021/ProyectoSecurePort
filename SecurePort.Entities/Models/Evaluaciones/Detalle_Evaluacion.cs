namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPEVA_Detalle_Evaluacion")]
    public class Detalle_Evaluacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Id_Evaluacion { get; set; }
        
        [Required]
        public int Id_Version_Eval { get; set; }

        public int? Id_Instalacion { get; set; }

        [Required]
        public int Id_Bien { get; set; }

        [Required]
        public int Id_Amenaza { get; set; }

        public bool? IND_IAS_ITP { get; set; }

        public int? NP1_IAI { get; set; }

        public int? NP1_ISD { get; set; }

        public int? NP1_IIO { get; set; }

        public int? NP1_IDV { get; set; }

        public int? NP1_IDE_D { get; set; }

        public int? NP1_IDE_I { get; set; }

        public int? NP1_IRD { get; set; }

        public int? NP1_IRR { get; set; }

        public int? NP1_ISA_MA { get; set; }

        public int? NP1_ISA_PA { get; set; }

        public int? NP1_ISA_AS { get; set; }

        public int? NP1_RESULTADO { get; set; }

        public int? NP1_ID { get; set; }

        public int? NP1_IC { get; set; }

        public int? NP1_IV { get; set; }

        public int? NP2_IAI { get; set; }

        public int? NP2_ISD { get; set; }

        public int? NP2_IIO { get; set; }

        public int? NP2_IDV { get; set; }

        public int? NP2_IDE_D { get; set; }

        public int? NP2_IDE_I { get; set; }

        public int? NP2_IRD { get; set; }

        public int? NP2_IRR { get; set; }

        public int? NP2_ISA_MA { get; set; }

        public int? NP2_ISA_PA { get; set; }

        public int? NP2_ISA_AS { get; set; }

        public int? NP2_RESULTADO { get; set; }

        public int? NP2_ID { get; set; }

        public int? NP2_IC { get; set; }

        public int? NP2_IV { get; set; }

        public int? NP3_IAI { get; set; }

        public int? NP3_ISD { get; set; }

        public int? NP3_IIO { get; set; }

        public int? NP3_IDV { get; set; }

        public int? NP3_IDE_D { get; set; }

        public int? NP3_IDE_I { get; set; }

        public int? NP3_IRD { get; set; }

        public int? NP3_IRR { get; set; }

        public int? NP3_ISA_MA { get; set; }

        public int? NP3_ISA_PA { get; set; }

        public int? NP3_ISA_AS { get; set; }

        public int? NP3_RESULTADO { get; set; }

        public int? NP3_ID { get; set; }

        public int? NP3_IC { get; set; }

        public int? NP3_IV { get; set; }

        public int? ID_Usu_alta { get; set; }

        [Required]
        public DateTime Fech_Alta { get; set; }

    }
}
