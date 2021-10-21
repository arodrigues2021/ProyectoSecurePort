namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class MOV_Detalle_InstalacionViewModel
    {

        public int Id { get; set; }
        
        public int Id_Instalacion { get; set; }

        public string IIPP { get; set; } 
        
        public int Id_Bien { get; set; }

        public string Bien { get; set; }

        public int? Id_BienPadre { get; set; }

        public string BienPadre { get; set; }
        
        public int? ID_Amenaza { get; set; }

        public string Amenaza { get; set; }

        public int? NumeroAmenazas { get; set; }
        
    }
}
