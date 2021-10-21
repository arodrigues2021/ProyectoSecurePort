namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    [Table("SPMAE_Puertos")]
    public class Puertos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id                   { get; set; }
        
        [Required]
        public string Nombre            { get; set; }
        
        [Required]
        public int id_Organismo         { get; set; }
        
        [Required]
        public string Responsable       { get; set; }
        
        public string Direccion         { get; set; }
        
        public int? ID_Ciudad           { get; set; }
        
        public int? Cod_Postal          { get; set; }
        
        public int? ID_Provincia        { get; set; }
        
        public int? ID_Usu_alta          { get; set; }
        
        [Required]
        public DateTime Fech_Alta       { get; set; }
        
        public Boolean? es_activo       { get; set; }
        
        public DateTime? fech_activo    { get; set; }
        
        public string Observaciones     { get; set; }
        
        public int? Latitud             { get; set; }
        
        public int? Longitud            { get; set; }
        
        public string Locode            { get; set; }
        
        public int? Id_CapMarit         { get; set; }

    }
}