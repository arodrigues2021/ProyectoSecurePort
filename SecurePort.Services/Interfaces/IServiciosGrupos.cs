using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{

    public interface IServiciosGrupos
    {
        IEnumerable<GruposPerfiles> GetAllPerfiles();

        IEnumerable<Grupos> GetAllGrupos();
    }

}
