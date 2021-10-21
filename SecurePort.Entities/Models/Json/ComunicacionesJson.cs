namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class ComunicacionesJson
    {
        public int Id { get; set; }

        public int Id_Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        public string Asunto { get; set; }

        public DateTime Fecha { get; set; }

        public string Emisor { get; set; }

        public string Receptor { get; set; }

        public string Mensaje { get; set; }

        public bool Recibido { get; set; }

        public string action { get; set; }
    }
}
