namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class MantenimientoJson
    {

        public int Id { get; set; }

        public int Id_Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public string Equipo { get; set; }

        public string Realizador { get; set; }

        public string Validador { get; set; }

        public DateTime? Fecha_Revision { get; set; }

        public string Observaciones { get; set; }

        public string action { get; set; }

        public string Validar { get; set; }

    }
}
