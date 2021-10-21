namespace SecurePort.Entities.Models
{
    using System;

    public class ListPaises
    {
        public int Id { get; set; }

        public string nombre { get; set; }

        public int? id_usu_alta { get; set; }

        public DateTime fech_alta { get; set; }

        public Boolean es_activo { get; set; }

        public DateTime fech_activo { get; set; }

        public string cod_pais { get; set; }

        public string activado { get; set; }

    }
}
