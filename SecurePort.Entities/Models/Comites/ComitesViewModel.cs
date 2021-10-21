namespace SecurePort.Entities.Models
{
    using System;

    public class ComitesViewModel
    {
        public int id { get; set; }

        public string Nombre_Organismo { get; set; }

        public string Nombre_Puerto { get; set; }

        public int Nivel { get; set; }

        public string NombreNivel { get; set; }

        public string Observaciones { get; set; }

        public DateTime Convocado { get; set; }

        public int Id_Organismo { get; set; }

        public int Id_Puerto { get; set; }

        public bool? activoOrg { get; set; }

        public bool? activoPue { get; set; }
    }
}
