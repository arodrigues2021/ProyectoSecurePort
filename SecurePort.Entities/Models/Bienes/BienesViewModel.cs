using System;
namespace SecurePort.Entities.Models
{
    public class BienesViewModel
    {
         public int Id { get; set; }

         public string Descripcion { get; set; }
         
         public int? id_Tipo_IIPP { get; set; }
         
         public int? id_Bien_Padre { get; set; }
        
         public Boolean es_activo { get; set; }
   
         public string Bien_Padre { get; set; }

         public string Tipo_instalacion { get; set; }

         public string activado { get; set; }

         public Boolean sel { get; set; }

    }
}