using System.Collections.Generic;
namespace SecurePort.Entities.Models
{

    public class CentrosViewModel
    {
        public int id { get; set; }

        public int? Id_Organismo { get; set; }

        public string Organismo { get; set; }

        public bool OrganismoActivo { get; set; }

        public int? Id_Puerto { get; set; }

        public string Puerto { get; set; }

        public string Centro_24H { get; set; }

        public string Operador { get; set; }

        public string Telefono { get; set; }

        public string Correo { get; set; }

        public  IEnumerable<OperadoresPuertoViewModel> operadorPuerto { get; set; }

        public string Direccion { get; set; }

        public int? Id_Ciudad { get; set; }

        public string Ciudad { get; set; }

        public int? Cod_Postal { get; set; }

        public int? Id_Provincia { get; set; }

        public string Provincia { get; set; }

    }
}
