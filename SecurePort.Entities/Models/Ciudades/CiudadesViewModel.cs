using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurePort.Entities.Models
{
    public class CiudadesViewModel
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

        public string isla { get; set; }

    }
}
