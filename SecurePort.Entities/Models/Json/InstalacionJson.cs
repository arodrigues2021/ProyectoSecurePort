namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class InstalacionJson
    {

        public int Id { get; set; }

        public string Nombre { get; set; }

        public int id_Puerto { get; set; }

        public int? id_Tipo { get; set; }

        public int es_concesionada { get; set; }

        public string Empresa { get; set; }

        public int Clasificacion { get; set; }

        public string Direccion { get; set; }

        public int? id_Ciudad { get; set; }

        public int? cod_postal { get; set; }

        public int? id_provincia { get; set; }

        public string OMI { get; set; }

        public string Nivel { get; set; }

        public int? Longitud { get; set; }

        public int? Latitud { get; set; }

        public string Declara_Cumpli { get; set; }

        public DateTime? Fech_Declara_Cumpli { get; set; }

        public string Activo { get; set; }

        public string action { get; set; }

        public string Observaciones { get; set; }

    }
}
