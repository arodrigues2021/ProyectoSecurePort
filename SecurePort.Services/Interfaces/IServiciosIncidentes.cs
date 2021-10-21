using System.Collections.Generic;
using SecurePort.Entities.Models;
using System;

namespace SecurePort.Services.Interfaces
{

    public interface IServiciosIncidentes
    {

        IEnumerable<IncidentesViewModel> GetAllIncidentes();

        IEnumerable<IncidentesViewModel> GetAllIncidentes(List<DependenciasUsuario> dependencias,int? categoria);

        IEnumerable<InstalacionesViewModel> GetInstalaciones(int IdPuerto, int? IdCategoria, List<DependenciasUsuario> dependencias);

        IEnumerable<Det_IncidenteViewModel> GetInstalacionesbyIncidente(int IdInciente);

        IEnumerable<MOV_Instalaciones> GetDependenciaInstalaciones(int id, int? idCategoria);
    }
}



