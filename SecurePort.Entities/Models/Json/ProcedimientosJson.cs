using System;
namespace SecurePort.Entities.Models.Json
{
    public class ProcedimientosJson
    {
        public int Id               { get; set; }

        public int Id_Organismo     { get; set; }

        public bool? Ind_AP         { get; set; }

        public string Titulo        { get; set; }

        public DateTime Fecha       { get; set; }

        public string Descripcion   { get; set; }

        public string Observaciones { get; set; }
        
        public string action        { get; set; }

        public string Validar       { get; set; }
        
    }
}


