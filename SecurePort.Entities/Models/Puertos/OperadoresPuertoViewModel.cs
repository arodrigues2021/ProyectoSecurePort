namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class OperadoresPuertoViewModel
    {

      
        public int Id { get; set; }
      
        public int Id_Puerto { get; set; }
        
        public string Puerto { get; set; }

        public string Nombre { get; set; }

        public Boolean Es_Suplente { get; set; }

        public string Telefono { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public int? Id_usu_alta { get; set; }

        public DateTime Fech_alta { get; set; }

        public Boolean Es_Activo { get; set; }
        
        public DateTime Fech_activo { get; set; }

        public string Observaciones { get; set; }

        public string activado_Sup { get; set; }

        public string Direccion { get; set; }

        public int? ID_Ciudad { get; set; }

        public string Ciudad { get; set; }

        public int? Cod_Postal { get; set; }

        public int? ID_Provincia { get; set; }
        
        public string Provincia { get; set; }

        public string activado { get; set; }

    }
}
