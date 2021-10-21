using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosCnae
    {
        IEnumerable<CNAE> GetAllCNAE();

        bool GetCNAE(string IdCodigo);
    }

}
