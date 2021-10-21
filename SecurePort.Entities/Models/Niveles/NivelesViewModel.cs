namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class NivelesViewModel
    {

        public int Id { get; set; }

        public int Id_Puerto { get; set; }

        public string Puerto { get; set; }

        public bool PuertoActivo { get; set; }

        public int? Id_IIPP { get; set; }

        public string IIPP { get; set; }

        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public string Emisor { get; set; }

        public string Receptor { get; set; }

        public int Nivel { get; set; }

        public string Emisor_Cancela { get; set; }

        public int? Duracion { get; set; }

        public DateTime? Fecha_Cancela { get; set; }

        public int? Nivel_Max { get; set; }

        public string Relevante { get; set; }

        public string Recomendacion { get; set; }

        public string Observaciones { get; set; }

    }
}
