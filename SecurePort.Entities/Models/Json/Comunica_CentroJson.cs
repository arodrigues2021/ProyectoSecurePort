using System;
namespace SecurePort.Entities.Models.Json
{
    public class Comunica_CentroJson
    {
        public int id { get; set; }

        public int Id_Centro24h { get; set; }

        public int Tipo_Canal { get; set; }

        public string Dato { get; set; }

        public int? id_usu_alta { get; set; }

        public DateTime fech_alta { get; set; }

        public string Nota { get; set; }

        public string action { get; set; }
    }
}


