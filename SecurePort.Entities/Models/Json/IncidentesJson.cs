namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class IncidentesJson
    {

        public int Id { get; set; }

        public int Id_Puerto { get; set; }

        public int? Id_IIPP { get; set; }
        
        public string Descripcion { get; set; }

        public int Tipo { get; set; }

        public DateTime Fecha { get; set; }
        
        public string Observaciones { get; set; }

        public string action { get; set; }
       
    }
}
