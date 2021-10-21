namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class FormacionesViewModel
    {

        public int Id { get; set; }

        public int Id_Organismo { get; set; }

        public string Organismo { get; set; }

        public int Id_Puerto { get; set; }

        public string Puerto { get; set; }

        public int? Id_IIPP { get; set; }

        public string IIPP { get; set; }

        public string Titulo { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fin { get; set; }

        public int Duracion { get; set; }

        public string Lugar { get; set; }

        public string Entidad { get; set; }

        public string Observaciones { get; set; }

    }
}
