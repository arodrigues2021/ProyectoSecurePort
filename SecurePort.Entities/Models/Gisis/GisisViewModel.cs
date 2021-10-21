namespace SecurePort.Entities.Models
{
    #region using
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class GisisViewModel
    {
        public int Id { get; set; }

        public int Id_Puerto { get; set; }

        public string Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        public string IIPP { get; set; }

        public int Tipo_Registro { get; set; }

        public string Registro { get; set; }

        public DateTime? Fecha_Registro { get; set; }

        public string Motivo { get; set; }

        public int? ID_Usu_alta { get; set; }

        public DateTime Fech_Alta { get; set; }

        
        
    }
}
