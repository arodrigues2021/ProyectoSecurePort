using System.Collections.Generic;
using SecurePort.Entities.Models;
using System;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosComites {

        IEnumerable<ComitesViewModel> GetAllComites();

        IEnumerable<ComitesViewModel> GetAllComites(List<DependenciasUsuario> dependencias);

        IEnumerable<Comites> ListComites();

        IEnumerable<Comites> ListComites(string nombre, int Id);

        bool ListComites(string nombre, int Id, bool estado);

        bool ListComites(int Organismo, int Puerto, DateTime fecha);

        bool ListComites(int Organismo, int Puerto, int ID, DateTime fecha);

    }
   
}
