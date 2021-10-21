
namespace SecurePort.Entities.Models
{

    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion


    public class ListadoCiudades
    {
        public int id { get; set; }

        public string codigo { get; set; }

        public string nombre { get; set; }

        public int Id_provincia { get; set; }

        public int? id_usu_alta { get; set; }

        public DateTime fech_alta { get; set; }

        public Boolean es_activo { get; set; }

        public DateTime fech_activo { get; set; }

        public string provincia { get; set; }

        public string activado { get; set; }

        public int? id_isla { get; set; }
                
    }
}
