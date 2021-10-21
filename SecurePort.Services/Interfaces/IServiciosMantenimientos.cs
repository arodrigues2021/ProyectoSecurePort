using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosTipoInstalacion
    {
        IEnumerable<ListadoTipoInstalaciones> ListTipoInstalaciones();

        IEnumerable<ListadoTipoInstalaciones> ListTipoInstalaciones(int? id);

        IEnumerable<Tipos_Instalacion> ListTipoInstalaciones(string codigo);

        IEnumerable<Tipos_Instalacion> ListTipoInstalaciones(string nombre, int Id);

        bool ListTipoInstalaciones(string nombre, int Id, bool estado);

        IEnumerable<InstalacionViewModel> GetAllInstalaciones();

        IEnumerable<InstalacionViewModel> GetAllInstalaciones(List<DependenciasUsuario> dependencias);

        IEnumerable<OperadoresViewModel> GetAllOperadores(int? id_instalcion);

        IEnumerable<MOV_Instalaciones> ListMOV_Instalaciones(string nombre);

        IEnumerable<MOV_Instalaciones> ListMOV_Instalaciones(string nombre, int Id);

        bool ListMOV_Instalaciones(string nombre, int Id, bool estado);

        IEnumerable<OperadoresViewModel> ListOperadores(int? id_instalcion);

        IEnumerable<MOV_Instalaciones> ListMOV_Instalaciones(int? Id);

        string GetClasifica(int? id);

    }
}
