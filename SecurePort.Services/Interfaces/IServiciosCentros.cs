using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
   using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosCentros
    {
        IEnumerable<Centros> GetAllCentros();

        bool ListCentros(string centro);

        bool ListCentros(string centro, int Id);

        IEnumerable<CentrosViewModel> ListAllCentros();

        IEnumerable<CentrosViewModel> ListAllCentros(List<DependenciasUsuario> dependencias);

        IEnumerable<Comunica_Centro> GetAdiocionalbyTipo(int TipoCanal, int ID);

        IEnumerable<Comunica_Centro> GetAdiocionalbyTipo(int ID);

        IEnumerable<OperadoresPuertoViewModel> GetAllOperadores(int IDCentro);
               
    }
}
