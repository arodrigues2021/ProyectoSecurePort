using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosBienes {

        IEnumerable<BienesViewModel> GetAllBienes();

        IEnumerable<BienesViewModel> GetAllBienesPadre(int? id);

        IEnumerable<BienesHijosViewModel> GetAllBienesHijos(int? Id);

        IEnumerable<BienesViewModel> ListBienes();

        IEnumerable<Bienes> ListBienes(string codigo);

        IEnumerable<Bienes> ListBienes(string nombre, int Id);

        bool ListBienes(string nombre, int Id, bool estado);

        IEnumerable<BienesHijosViewModel> GetAllBienesHijosAmenazas(int? id,int?idInstalacion);

        IEnumerable<Bienes> ListBienesHijos(int? IdPadre);

    }
   
}
