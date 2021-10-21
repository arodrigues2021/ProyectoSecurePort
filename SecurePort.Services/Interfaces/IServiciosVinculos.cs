using System.Collections.Generic;
using SecurePort.Entities.Models;
using System;

namespace SecurePort.Services.Interfaces
{

    public interface IServiciosVinculos
    {

        IEnumerable<Vinculos> GetAllVinculos(string Tipo);

        IEnumerable<Vinculos> GetAllVinculos(string Tipo, int? IdCategoria);

    }
}



