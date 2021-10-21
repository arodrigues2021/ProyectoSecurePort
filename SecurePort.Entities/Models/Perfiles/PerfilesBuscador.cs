namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class PerfilesBuscador
    {
        public int Id_perfil { get; set; }
       
        public string Nombre { get; set; }

        public string Description { get; set; }

    }
}
