using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurePort.Entities.Models
{
     public class IncidentesViewModel
    {
         public int Id { get; set; }

         public string Puerto { get; set; }

         public int Id_Puerto { get; set; }

         public string IIPP { get; set; }

         public int? Id_IIPP { get; set; }

         public string NombreTipo { get; set; }

         public int Tipo { get; set; }

         public string Descripcion { get; set; }
        
         public DateTime Fecha { get; set; }

         public string Observaciones { get; set; }
         
    }
}
