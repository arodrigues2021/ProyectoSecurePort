using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosMotivos {

        IEnumerable<MotivosViewModel> GetAllMotivos();

        IEnumerable<MotivosViewModel> ListMotivos();

        IEnumerable<Motivos> ListMotivos(string codigo);

        IEnumerable<Motivos> ListMotivos(string nombre, int Id);

        bool ListMotivos(string nombre, int Id, bool estado);

    }
   
}
