using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosPracticas {

        IEnumerable<PracticasViewModel> ListPracticas();

        IEnumerable<PracticasViewModel> ListPracticas(List<DependenciasUsuario> dependencias,int? categoria);

        IEnumerable<InstalacionesViewModel> GetInstalaciones(int IdPuerto, int? IdCategoria, List<DependenciasUsuario> dependencias);

        IEnumerable<Det_PracticaViewModel> GetInstalacionesbyPractica(int IdPractica);

        IEnumerable<MOV_Instalaciones> GetDependenciaInstalaciones(int id, int? idCategoria);

        }
}
