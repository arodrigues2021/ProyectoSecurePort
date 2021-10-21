using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosAuditorias {

        IEnumerable<AuditoriasViewModel> ListAuditorias();

        IEnumerable<AuditoriasViewModel> ListAuditorias(List<DependenciasUsuario> dependencias,int? categoria);


    }
}
