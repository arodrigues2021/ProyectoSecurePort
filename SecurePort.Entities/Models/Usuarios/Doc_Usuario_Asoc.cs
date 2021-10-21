namespace SecurePort.Entities.Models
{
    using System;

    public class Doc_Usuario_Asoc
    {
        public int id { get; set; }

        public int id_servicio { get; set; }

        public int id_tipo_serv { get; set; }

        public int id_tipo_doc { get; set; }

        public string documento { get; set; }

        public string descripcion { get; set; }

        public DateTime fech_documento { get; set; }

        public string TipoNombre { get; set; }

    }

}
