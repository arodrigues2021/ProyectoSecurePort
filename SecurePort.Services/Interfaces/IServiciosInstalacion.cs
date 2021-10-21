using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosTipoInstalacion
    {
        IEnumerable<ListadoTipoInstalaciones> ListTipoInstalaciones();

        IEnumerable<ListadoTipoInstalaciones> ListTipoInstalaciones(int? id);

        IEnumerable<ListadoTipoInstalaciones> ListInstalaciones(int? id);

        IEnumerable<Tipos_Instalacion> ListTipoInstalaciones(string codigo);

        IEnumerable<Tipos_Instalacion> ListTipoInstalaciones(string nombre, int Id);

        bool ListTipoInstalaciones(string nombre, int Id, bool estado);

        IEnumerable<InstalacionViewModel> GetAllInstalaciones();

        IEnumerable<InstalacionViewModel> GetAllInstalaciones(List<DependenciasUsuario> dependencias,int? categoria);

        IEnumerable<OperadoresViewModel> GetAllOperadores(int? id_instalcion);

        IEnumerable<MOV_Instalaciones> ListMOV_InstalacionesVerifica(string nombre, int IdPuerto);

        IEnumerable<MOV_Instalaciones> ListMOV_Instalaciones(string nombre, int Id);

        bool ListMOV_Instalaciones(string nombre, int Id, bool estado);

        IEnumerable<OperadoresViewModel> ListOperadores(int? id_instalcion);

        IEnumerable<MOV_Instalaciones> ListMOV_Instalaciones(int? Id);

        string GetClasifica(int? id);

        IEnumerable<MOV_Detalle_InstalacionViewModel> GetAllBienesByInstalacion(int idInstalacion);

        IEnumerable<AmenazasViewModel> GetAllBienesByInstalacionBien(int idInstalacion, int idBien);

    }
}
