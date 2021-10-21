using System.Collections.Generic;
using SecurePort.Entities.Models;
using System;

namespace SecurePort.Services.Interfaces
{

    public interface IServiciosNiveles
    {

        IEnumerable<NivelesViewModel> GetAllNiveles();

        IEnumerable<NivelesViewModel> GetAllNiveles(List<DependenciasUsuario> dependenciasint,int? idCategoria);

        IEnumerable<InstalacionesViewModel> GetInstalaciones(int IdPuerto, int? IdCategoria, List<DependenciasUsuario> dependencias);

        IEnumerable<Det_NivelViewModel> GetInstalacionesbyNivel(int IdNivel);

        IEnumerable<MOV_Instalaciones> GetDependenciaInstalaciones(int id, int? idCategoria);

    }
}



