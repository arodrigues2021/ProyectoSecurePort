using System.Collections.Generic;
using SecurePort.Entities.Models;
using System;

namespace SecurePort.Services.Interfaces
{

    public interface IServiciosEvaluaciones
    {

        IEnumerable<EvaluacionesViewModel> GetAllEvaluaciones();

        IEnumerable<EvaluacionesViewModel> GetAllEvaluaciones(List<DependenciasUsuario> dependencias,int? categoria);

        IEnumerable<Historico_Evaluacion> GetEvaluacionesByIdEvaluaVersion(int idEvaluacion, int IdVersion);

        IEnumerable<Versiones_Evaluacion> GetAllVersiones(int IdEvaluacion);

        IEnumerable<Detalle_EvaluacionViewModel> GetAllBienesByInstalacionBienDetail(int idInstalacion, int idBien);

        IEnumerable<Detalle_EvaluacionViewModel> GetAllBienesByInstalacionBienEvaluacionDetail(int idInstalacion, int idBien, int IdEvaluacion);
    }
}



