namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion


    public class ContactosViewModel
    {

        
        public int Id { get; set; }

        public int Id_Organismo { get; set; }

        public string Nombre { get; set; }

        public Boolean Es_Responsable { get; set; }

        public string Telefono { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public int? ID_Usu_Alta { get; set; }

        public DateTime fech_alta { get; set; }

        public Boolean es_activo { get; set; }

        public DateTime fech_activo { get; set; }

        public string Observaciones { get; set; }

        public string Cargo { get; set; }

        public string activado { get; set; }


    }
}
