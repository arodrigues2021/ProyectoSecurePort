namespace SecurePort.Entities.Models
{
    #region using
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    #endregion

    public class DependenciasUsuario
    {
        public int? id_Usuario { get; set; }

        public int? categoria { get; set; }

        public int id_Dependencia { get; set; }

        public string Nombre_Dependencia { get; set; }
    
    }
}
