
namespace SecurePort.Entities.Models
{

    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion


    public class ListadoPaises
    {
        public int id { get; set; }

        public string nombre { get; set; }

        public int? id_usu_alta { get; set; }

        public DateTime fech_alta { get; set; }

        public Boolean es_activo { get; set; }

        public DateTime fech_activo { get; set; }

        public string cod_pais { get; set; }

    }
}
