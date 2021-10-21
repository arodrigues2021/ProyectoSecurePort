namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class DeclaraMaritimaViewModel
    {
        public int Id { get; set; }

        public int Id_Organismo { get; set; }

        public string NombreOrganismo { get; set; }

        public int? Id_Puerto { get; set; }

        public string NombrePuerto { get; set; }

        public int Id_IIPP { get; set; }

        public string NombreIIPP { get; set; }

        public string IMO { get; set; }

        public string Buque { get; set; }

        public int Id_Motivo { get; set; }
        
        public DateTime Fech_Expe { get; set; }

        public DateTime Fech_Ini_Validez { get; set; }

        public DateTime Fech_Fin_Validez { get; set; }

        public string Responsable { get; set; }

        public int? Nivel_Buq { get; set; }

        public int? Nivel_IIPP { get; set; }

        public string Medidas { get; set; }

        public string Observaciones { get; set; }
        
    }
}
