namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class AuditoriasJson
    {
        public int Id { get; set; }

        public int Id_Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        public string Descripcion { get; set; }

        public int Tipo { get; set; }

        public int Estado { get; set; }

        public DateTime? Ini_Programa { get; set; }

        public DateTime? Fin_Programa { get; set; }

        public DateTime? Ini_Planifica { get; set; }

        public DateTime? Fin_Planifica { get; set; }

        public DateTime? Ini_Real { get; set; }

        public DateTime? Fin_Real { get; set; }

        public string Responsable { get; set; }

        public string Conclusiones { get; set; }

        public string Recomendaciones { get; set; }

        public string Programa { get; set; }

        public string Seguimiento { get; set; }

        public string Observaciones { get; set; }

        public string action { get; set; }

        public string Validar { get; set; }
        
    }
}
