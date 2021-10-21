namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

   public class GruposViewModel
    {
        public int Id { get; set; }
       
        public string Nombre { get; set; }
       
        public string descripcion { get; set; }

        public int? Id_publico { get; set; }

        public string es_activo { get; set; }

    }
}
