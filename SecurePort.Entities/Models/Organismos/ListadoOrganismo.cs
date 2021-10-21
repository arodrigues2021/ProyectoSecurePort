namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class ListadoOrganismo
    {

        public int id { get; set; }

        public int IdOrganismo { get; set; }

        public string Organismo { get; set; }

        public string Tipo { get; set; }

        public string responsable { get; set; }

        public string Direccion { get; set; }

        public string Ciudad { get; set; }

        public string CodigoPostal { get; set; }

        public string Provincia { get; set; }

        public string comunidad { get; set; }

        public bool Activo { get; set; }

        public string activado { get; set; }

    }
}

