
namespace SecurePort.Entities.Models
{

    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion


    public class ListadoProvincias
    {
        public int Id { get; set; }

        public string codigo { get; set; }
        
        public string nombre { get; set; }

        public string pais { get; set; }

        public string Id_pais { get; set; }

        public string comunidad { get; set; }

        public int? id_usu_alta { get; set; }

        public DateTime fech_alta { get; set; }

        public Boolean Activo { get; set; }

        public DateTime fech_activo { get; set; }

        public int ID_ComAut { get; set; }

        public string activado { get; set; }
    }
}
