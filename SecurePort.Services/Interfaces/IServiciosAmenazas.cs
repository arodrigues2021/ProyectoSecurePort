using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosAmenazas {

        IEnumerable<AmenazasViewModel> GetAllAmenazas();

        IEnumerable<AmenazasViewModel> ListAmenazas();

        IEnumerable<Amenazas> ListAmenazas(string codigo);

        IEnumerable<Amenazas> ListAmenazas(string nombre, int Id);

        bool ListAmenazas(string nombre, int Id, bool estado);

        IEnumerable<AmenazasViewModel> GetAllAmenazasPadreHijos();

    }
   
}
