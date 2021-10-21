
namespace SecurePort.Entities.Models
{

    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion


    public class ListadoTipoInstalaciones
    {
        public int id { get; set; }

        public string descripcion { get; set; }

        public int? clasificacion { get; set; }

        public string Nombreclasificacion { get; set; }

        public int? id_usu_alta { get; set; }

        public DateTime fech_alta { get; set; }

        public Boolean es_activo { get; set; }

        public DateTime fech_activo { get; set; }

        public string activado { get; set; }

    }
}
