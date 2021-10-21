namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class ListadoContactos_Organismo
    {
        public int Id { get; set; }

        public string Contacto { get; set; }

        public string Es_Responsable { get; set; }

        public string Cargo { get; set; }

        public string Telefono { get; set; }

        public string Fax { get; set; }

        public string Correo { get; set; }

        public string Observaciones { get; set; }
        
    }
}
