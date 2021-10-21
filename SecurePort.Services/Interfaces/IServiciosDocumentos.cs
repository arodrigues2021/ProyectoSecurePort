using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosDocumentos {

        IEnumerable<Doc_Asoc> GetDocs(int? id, int tipo_Servicio);

        IEnumerable<Tipos_Documento> GetTiposDocumento();


        }
}

